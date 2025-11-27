using System;
using System.ComponentModel.DataAnnotations;
using WebGoatCore.DomainPrimitives;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public Quantity Quantity { get; set; } // Changed to Quantity primitive
        public float Discount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        public decimal DecimalUnitPrice => Convert.ToDecimal(this.UnitPrice);
        public decimal ExtendedPrice => DecimalUnitPrice * Convert.ToDecimal(1 - Discount) * Quantity.Value;
    }
}
