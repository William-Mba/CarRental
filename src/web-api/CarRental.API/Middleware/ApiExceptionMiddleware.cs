using CarRental.API.Utils;
using Serilog;
using System.Text.Json;

namespace CarRental.API.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ApiExceptionOptions _options;

        public ApiExceptionMiddleware(ApiExceptionOptions options, RequestDelegate next)
        {
            _options = options;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var error = new ApiError
            {
                Id = Guid.NewGuid().ToString(),
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occured in the API. Please use the id and contact our support team if the problem persists."
            };

            _options.AddResponseDetails?.Invoke(context, exception, error);

            var innerExMessage = exception.GetBaseException().Message;

            Log.Error(exception, "API Error: " + "{ErrorMessage} -- {ErrorId}.", innerExMessage, error.Id);

            var result = JsonSerializer.Serialize(error);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync(result);
        }
    }
}
