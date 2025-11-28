using WebGoatCore.Models;
using WebGoatCore.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using WebGoatCore.DomainPrimitives;
using Microsoft.AspNetCore.Http.HttpResults;
using WebGoatCore.ViewModels;

namespace WebGoatCore.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly ProductRepository _productRepository;

        public CartController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        private Cart GetCart()
        {
            HttpContext.Session.TryGet<Cart>("Cart", out var cart);
            if(cart == null)
            {
                cart = new Cart();
            }
            return cart;
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost("{productId}")]
        public IActionResult AddOrder(int productId, short quantity) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(StatusCodeViewModel.Create(new ApiResponse(400)));
            }

            Result<Quantity> result = Quantity.Create(quantity);
            if (!result.IsSuccessful)
            {
                return BadRequest(StatusCodeViewModel.Create(new ApiResponse(400, result.Error_msg)));
            }

            var product = _productRepository.GetProductById(productId);
            
            var cart = GetCart();
            if(!cart.OrderDetails.ContainsKey(productId))
            {
                var orderDetail = new OrderDetail()
                {
                    Discount = 0.0F,
                    ProductId = productId,
                    Quantity = result.Value, // Using Quantity primitive
                    Product = product,
                    UnitPrice = product.UnitPrice
                };
                cart.OrderDetails.Add(orderDetail.ProductId, orderDetail);
            }
            else
            { // Hele denne her kodeblok overholder ikke SRP
                OrderDetail originalOrder = cart.OrderDetails[productId];
                
                int newQuantityValue = originalOrder.Quantity.Value + result.Value.Value; // Short + Short converts it to Int to avoid overflow issues
                
                if (newQuantityValue > short.MaxValue || newQuantityValue < short.MinValue)
                {
                    return BadRequest("Quantity would exceed allowed range");
                }
                
                originalOrder.Quantity = Quantity.Create((short)newQuantityValue).Value;
            }

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index");
        }

        [HttpGet("{productId}")]
        public IActionResult RemoveOrder(int productId)
        {
            try
            {
                var cart = GetCart();
                if (!cart.OrderDetails.ContainsKey(productId))
                {
                    return View("RemoveOrderError", string.Format("Product {0} was not found in your cart.", productId));
                }

                cart.OrderDetails.Remove(productId);
                HttpContext.Session.Set("Cart", cart);

                Response.Redirect("~/ViewCart.aspx");
            }
            catch (Exception ex)
            {
                return View("RemoveOrderError", string.Format("An error has occurred: {0}", ex.Message));
            }

            return RedirectToAction("Index");
        }
    }
}