namespace TelegramBotExperiments
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value;
            _logger.LogInformation("Request Method: {Method}", context.Request.Method);
            _logger.LogInformation("number of page transitions {Method}",
                context.Request.Path + context.Request.Path.Value);
            await _next(context);
        }
    }
}