using System.Net;
using System.Text;
using Ganss.Xss;
using Newtonsoft.Json;
using SearchAPI.Common.Classes.Dto;

namespace SearchAPI.Middleware
{
    public class AntiXssMiddleware:IMiddleware
    {
        private readonly ILogger<AntiXssMiddleware> _logger;
        private readonly int _statusCode = (int)HttpStatusCode.BadRequest;
        Response _response;
        public AntiXssMiddleware(ILogger<AntiXssMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var originalBody = context.Request.Body;
            try
            {
                var content = await ReadRequestBody(context);

                var sanitiser = new HtmlSanitizer();
                var sanitised = sanitiser.Sanitize(content);
                if (content != sanitised.Replace("&amp;", "&"))
                {
                    await RespondWithAnError(context).ConfigureAwait(false);
                }

                await next(context);
            }
            finally
            {
                context.Request.Body = originalBody;
            }

        }

        private static async Task<string> ReadRequestBody(HttpContext context)
        {
            var buffer = new MemoryStream();
            await context.Request.Body.CopyToAsync(buffer);
            context.Request.Body = buffer;
            buffer.Position = 0;

            var encoding = Encoding.UTF8;

            var requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
            context.Request.Body.Position = 0;

            return requestContent;
        }
        private async Task RespondWithAnError(HttpContext context)
        {
            context.Response.Clear();
            context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = _statusCode;

            if (_response == null)
            {
                _response = new Response
                {
                    Message = "XSS Detected.",
                    Status = "Error"
                };
            }

            await context.Response.WriteAsync(JsonConvert.SerializeObject(_response));
        }
    }

    public static class AntiAxxMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiXss(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AntiXssMiddleware>();
        }
    }


}
