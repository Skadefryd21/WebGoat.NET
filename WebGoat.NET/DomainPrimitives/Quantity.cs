using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.DomainPrimitives
{
    public class Quantity
    {
        private ushort value { get; set; }

        public Quantity(ushort value)
        {
            if (value == 0)
            {
                return new Result.Result<Quantity>(false, "Quantity cannot be 0");
            }

            this.value = value;
        }
    }
}