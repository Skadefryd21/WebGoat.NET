using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.DomainPrimitives
{
    public class Quantity
{
    private ushort value { get; set; }

    public static Result.Result<Quantity> Create(ushort value)
    {
        if (value == 0)
        {
            return Result.Result<Quantity>.Failure("Quantity må ikke være 0");
        }

        return Result.Result<Quantity>.SuccessResult(new Quantity { value = value });
    }

    public ushort GetValue()
    {
        return value;
    }
}
}