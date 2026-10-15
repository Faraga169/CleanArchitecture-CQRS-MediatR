using System.Net;
using Products.Application.Exceptions;

namespace Products.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Business exception occurred. StatusCode: {StatusCode}",
                    ex.StatusCode);

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = ex.StatusCode
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred while processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}