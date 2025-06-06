using System.Text.Json;
using System.Text.RegularExpressions;

namespace ApiGateway.Middleware
{
    public class ErrorWrappingApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorWrappingApiMiddleware> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ErrorWrappingApiMiddleware(RequestDelegate next, ILogger<ErrorWrappingApiMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            var message = string.Empty;
            var messageDetail = string.Empty;
            object response;
            try
            {
                await _next.Invoke(context);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning("ErrorWrappingMiddleware OperationCanceledException {0} {1}", ex.Message, ex);

                message = Regex.Replace("Response timeout, please try again later.", @"\t|\n|\r", "");
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.GatewayTimeout;
            }
            catch (Exception ex)
            {
                _logger.LogError("ErrorWrappingMiddleware {0} {1}", ex.Message, ex);

                message = Regex.Replace(ex.Message, @"\t|\n|\r", "");

                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                string json = "";
                response = new
                {
                    err = "An error occurred. Please try again."
                };

                json = JsonSerializer.Serialize(response, _jsonSerializerOptions);

                context.Response.Headers.Remove("Content-Length");
                await context.Response.WriteAsync(json);
            }
        }
    }
}