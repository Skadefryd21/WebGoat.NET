using System;
using System.ComponentModel.DataAnnotations;
using AspNetCoreGeneratedDocument;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.DomainPrimitives
{
    public class Result<T>
    {
        public bool IsSuccessful { get; private set; }
        public string Error_msg { get; private set; }
        public T Value { get; private set; }

        private Result(bool isSuccessful, string error_msg, T value = default) // Defaults to null for complex datatypes
        {
            this.IsSuccessful = isSuccessful;
            this.Error_msg = error_msg;
            this.Value = value;
        }

        public static Result<T> Success(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return new Result<T>(true, null, value);
        }

        public static Result<T> Failure(string error_msg)
        {
            ArgumentNullException.ThrowIfNull(error_msg);
            return new Result<T>(false, error_msg, default);
        }
    }
}