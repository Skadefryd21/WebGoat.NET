using System;
using System.ComponentModel.DataAnnotations;
using AspNetCoreGeneratedDocument;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
namespace WebGoatCore.Result
{
    public class Result<T>
    {
        public bool Success = true;
        public string Error_msg = "";

        public Result(bool success, string error_msg)
        {
            this.Success = success;
            this.Error_msg = error_msg;
        }
        public static Result<T> SuccessResult(T value)
        {
            return new Result<T>(true, "");
        }

        public static Result<T> Failure(string error_msg)
        {
            return new Result<T>(false, error_msg);
        }
    }
    
}