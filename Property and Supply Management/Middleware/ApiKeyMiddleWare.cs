namespace Property_and_Supply_Management.Middleware
{
	public class ApiKeyMiddleWare
	{
        private readonly string _apiKey;
        private readonly RequestDelegate _next;
        public ApiKeyMiddleWare(IConfiguration configuration, RequestDelegate next)
        {
            _next = next;
            _apiKey = configuration["AppSettings:Key"];
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if(!httpContext.Request.Headers.TryGetValue("X-API-KEY",out var extracted_key))
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsJsonAsync("Api key is missing");
                return;
            }

            if (!_apiKey.Equals(extracted_key))
            {
				httpContext.Response.StatusCode = 400;
				await httpContext.Response.WriteAsJsonAsync("Api key not matched");
                return;
			}

            await _next(httpContext);
        }
    }
}
