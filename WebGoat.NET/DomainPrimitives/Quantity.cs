using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.DomainPrimitives
{
    public record class Quantity
    {
        public short Value { get; init; } // Init can only be set via constructor, afterwards it is immutable

        private Quantity(short value) // Constructor is private to force usage of Create()
        {
            this.Value = value;
        } 

        public static Result<Quantity> Create(short value)
        {
            if (value <= 0)
            {
                return Result<Quantity>.Failure("Der må ikke bestilles 0 varer");
            }

            return Result<Quantity>.Success(new Quantity(value));
        }
    }
}