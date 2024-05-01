using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.services.CacheService;
using System.Text;

namespace API.Helpers
{

    // We Make Here custome attribute so we can put it above our endpoint
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute(int TimeToLiveInSeconds) {
            _timeToLiveInSeconds = TimeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await cacheService.GetCacheResponseAsync(cacheKey);

            if(!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };


                context.Result = contentResult;

                return;
            }

            var executedContext = await next();

            if(executedContext.Result is OkObjectResult response)
                await cacheService.SetCacheResponseAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var cachekey = new StringBuilder();

            cachekey.Append($"{request.Path}");

                        // tuple
            foreach(var (key, value) in request.Query.OrderBy(x => x.Key)) 
            {
                cachekey.Append($"|{key}-{value}");
            }

            return cachekey.ToString();
        }

    }
}
