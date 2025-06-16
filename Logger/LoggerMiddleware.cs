using System.Text;

namespace EzhikLoader.Server.Logger
{
    public class LoggerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next, ILogger logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestLogText = new StringBuilder();
            requestLogText.AppendLine($"Method: {context.Request.Method}");
            requestLogText.AppendLine($"Path: {context.Request.Path}");
            requestLogText.AppendLine($"User-Agent: {context.Request.Headers.UserAgent}");
            requestLogText.AppendLine($"Time: {DateTime.Now.ToLongTimeString()}");
            requestLogText.AppendLine($"Host: {context.Request.Host}");
            requestLogText.AppendLine($"Authorization: {context.Request.Headers.Authorization}");

            if (context.Request.Body.CanRead)
            {
                if (context.Request.ContentType != "multipart/form-data")
                {
                    using (var s = new StreamReader(context.Request.Body))
                    {
                        string body = await s.ReadToEndAsync();
                        requestLogText.AppendLine($"Body: {body.ReplaceLineEndings().Replace(" ", "")}");
                        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body), true);
                    }
                }
            }

            _logger.LogInformation(requestLogText.ToString());

            await _next.Invoke(context);

            var responseLogText = new StringBuilder();
            responseLogText.AppendLine($"StatusCode: {context.Response.StatusCode}");

            if (context.Response.Body.CanRead)
            {
                if (context.Request.ContentType != "multipart/form-data")
                {
                    responseLogText.AppendLine($"ContentDisposition: {context.Response.Headers.ContentDisposition}");

                    using (var s = new StreamReader(context.Response.Body))
                    {
                        string body = await s.ReadToEndAsync();
                        responseLogText.AppendLine($"Body: {body.ReplaceLineEndings().Replace(" ", "")}");
                        context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(body), true);
                    }
                }
            }

            _logger.LogInformation(responseLogText.ToString());
        }
    }

    public static class LoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder, ILogger logger)
        {
            return builder.UseMiddleware<LoggerMiddleware>(logger);
        }
    }
}
