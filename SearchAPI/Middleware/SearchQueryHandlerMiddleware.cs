using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SearchAPI.Common.Classes.Dto;
using SearchAPI.Models;
using SearchAPI.Repository;
using System.Threading.Tasks;

namespace SearchAPI.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SearchQueryHandlerMiddleware : IMiddleware
    {
        private IRepository<SearchHistory> _repoSearchHist;
        public SearchQueryHandlerMiddleware(IRepository<SearchHistory> repoSearchHist)
        {
            this._repoSearchHist = repoSearchHist;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (context.Request.QueryString.HasValue && context.Request.QueryString.Value.ToLower().Contains("$filter"))
                {
                    DateTime start = DateTime.Now;
                    await next(context);

                    if (context.Response.StatusCode == StatusCodes.Status200OK)
                    {
                        //var queryitems = context.Request.Query.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();
                        //List<KeyValuePair<string, string>> queryparameters = new List<KeyValuePair<string, string>>();
                        //foreach (var item in queryitems)
                        //{
                        //    var value = item.Value.ToString().Replace("x", "y");
                        //    KeyValuePair<string, string> newqueryparameter = new KeyValuePair<string, string>(item.Key, value);
                        //    queryparameters.Add(newqueryparameter);
                        //}
                        Microsoft.Extensions.Primitives.StringValues queryVal;
                        context.Request.Query.TryGetValue("$filter", out queryVal);
                        string condition = queryVal.FirstOrDefault();
                        

                        SearchHistory history = new SearchHistory()
                        {
                            SearchStarted = start,
                            SearchCompleted = DateTime.Now,
                            SearchCondition = condition,
                            RequestedIP = context.Connection.RemoteIpAddress.ToString(),
                            SearchStatus = "Success"
                        };

                        _repoSearchHist.Insert(history);
                        _repoSearchHist.Save();
                    }

                }
                else
                {
                    using (MemoryStream buffer = new MemoryStream())
                    {
                        Stream stream = context.Response.Body;
                        context.Response.Body = buffer;
                        buffer.Seek(0, SeekOrigin.Begin);
                        var reader = new StreamReader(buffer);
                        using (StreamReader bufferReader = new StreamReader(buffer))
                        {
                            string body = await bufferReader.ReadToEndAsync();
                            Response response = new Response()
                            {
                                Message = "Search condition is unavailable.",
                                Status = "Bad Request"
                            };

                            var jsonString = JsonConvert.SerializeObject(response);



                            // Added new code
                            await context.Response.WriteAsync(jsonString);
                            context.Response.Body.Seek(0, SeekOrigin.Begin);

                            await context.Response.Body.CopyToAsync(stream);
                            context.Response.Body = stream;
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
            }
        }
       
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SearchQueryHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseSearchQueryHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SearchQueryHandlerMiddleware>();
        }
    }
}
