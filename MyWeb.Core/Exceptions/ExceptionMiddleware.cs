namespace MyApp.Web.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using MyApp.Web.Filter;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception? ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            object result;
            if (ex is ModelValidationException mvE)
            {
                result = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Model validation failed.",
                    Detailed = mvE.Errors
                };
            }
            else
            {
                result = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error."
                };
            }
            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
        }
    }

}
