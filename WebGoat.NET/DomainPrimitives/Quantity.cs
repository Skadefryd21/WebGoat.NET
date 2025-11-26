using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.DomainPrimitives
{
    public record class Quantity
    {
        public ushort value { get; init; } // Init can only be set via constructor, afterwards it is immutable

        private Quantity(ushort value) // Constructor is private to force usage of Create()
        {
            this.value = value;
        } 

        public static Result<Quantity> Create(ushort value)
        {
            if (value == 0)
            {
                return Result<Quantity>.Failure("Der må ikke bestilles 0 varer");
            }

            return Result<Quantity>.Success(new Quantity(value));
        }
    }
}