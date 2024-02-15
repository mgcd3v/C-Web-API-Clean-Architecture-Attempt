using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace Shared.Middlewares
{
    public class BaseCustomException : Exception
    {
        private int _code;
        private string _description;

        public int Code
        {
            get => _code;
        }
        public string Description
        {
            get => _description;
        }

        public BaseCustomException(string message, string description, int code) : base(message)
        {
            _code = code;
            _description = description;
        }
    }

    public class CustomErrorResponse
    {
        public string? Message { get; set; }
        public string? Description { get; set; }
    }

    public class NotFoundCustomException : BaseCustomException
    {
        public NotFoundCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.NotFound)
        {
        }
    }

    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customException = exception as BaseCustomException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            //var message = "Unexpected error";
            var message = exception.Message;
            var description = "Unexpected error";

            if (null != customException)
            {
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                Message = message,
                Description = description
            }));
        }
    }
}
