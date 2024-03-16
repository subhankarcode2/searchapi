using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SearchAPI.Controllers;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchAPI.Middleware
{

    public class RequestResponseLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        public RequestResponseLoggingMiddleware(ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            
                Guid guid = Guid.NewGuid();
                _logger.LogTrace($"\n\nHTTP request information:\n" +
                $"\tTrace ID: {guid.ToString()}\n" +
                $"\tMethod: {context.Request.Method}\n" +
                $"\tPath: {context.Request.Path}\n" +
                $"\tQueryString: {context.Request.QueryString}\n" +
                $"\tHeaders: {FormatHeaders(context.Request.Headers)}\n" +
                $"\tSchema: {context.Request.Scheme}\n" +
                $"\tHost: {context.Request.Host}\n" +
                $"\tBody: {await ReadBodyFromRequest(context.Request)}\n\n");

                var originalResponseBody = context.Response.Body;
                using var newResponseBody = new MemoryStream();
                context.Response.Body = newResponseBody;

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                context.Response.StatusCode = 500;
            }

            newResponseBody.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();

                _logger.LogTrace($"\n\nHTTP response information:\n" +
                    $"\tTrace ID: {guid.ToString()}\n" +
                    $"\tStatusCode: {context.Response.StatusCode}\n" +
                    $"\tContentType: {context.Response.ContentType}\n" +
                    $"\tHeaders: {FormatHeaders(context.Response.Headers)}\n" +
                    $"\tBody: {responseBodyText}\n\n");

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalResponseBody);

        }


        private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{<!-- -->{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            request.EnableBuffering();

            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();

            request.Body.Position = 0;
            return requestBody;
        }

    }

    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }

}
