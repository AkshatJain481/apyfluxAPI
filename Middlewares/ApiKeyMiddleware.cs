namespace ImageDetector.Middlewares
{
    using ImageDetector.Services;

    public class ApiKeyMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, ApiKeyService apiKeyService)
        {
            if (
                !context.Request.Headers.TryGetValue("api-key", out var extractedApiKey)
                || !apiKeyService.IsValidApiKey(extractedApiKey)
            )
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            await _next(context);
        }
    }
}
