using System.Text.Json;

namespace Property_and_Supply_Management.Middleware
{
    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //enable buffering to read the request
            httpContext.Request.EnableBuffering();

            //log the request
            var request_text = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            httpContext.Request.Body.Position = 0;
            _logger.LogInformation($"Incoming Request: Path:{httpContext.Request.Path}\nMethod:{httpContext.Request.Method}\nBody:\n{request_text}");


            //hijack the response
            var original_body = httpContext.Response.Body;
            using var temp_body = new MemoryStream();
            httpContext.Response.Body = temp_body;


            //move to the next middleware
            await _next(httpContext);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var response_text = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

            try
            {
                if (!string.IsNullOrWhiteSpace(response_text) && response_text.TrimStart().StartsWith("{") || response_text.TrimStart().StartsWith("["))
                {
                    var formatted_response = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(response_text), new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    });

                    _logger.LogInformation($"Outgoing response: \nStatusCode:{httpContext.Response.StatusCode} \nBody:\n{formatted_response}");
                }
                else
                {
                    _logger.LogInformation($"Outgoing response: \nStatusCode:{httpContext.Response.StatusCode} \nBody:\n{response_text}");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex.ToString());
            }
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            await temp_body.CopyToAsync(original_body);
        }
    }
}
