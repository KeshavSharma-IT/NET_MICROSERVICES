using Microsoft.AspNetCore.Http;

namespace OrderMicroservice.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                _requestDelegate(httpContext);
            }
            catch (Exception ex) {

                _logger.LogError($"{ex.GetType().ToString()}: {ex.Message}");

                if (ex.InnerException is not null)
                {
                    _logger.LogError($"{ex.InnerException.GetType()}:{ex.InnerException.Message}");
                }
                httpContext.Response.StatusCode = 500;

                await httpContext.Response.WriteAsJsonAsync(new { Message = ex.Message, Type = ex.GetType().ToString() });
            }

        }
    }

    // Move the extension method to a top-level static class
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
