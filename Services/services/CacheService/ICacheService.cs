using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services.CacheService
{
    public interface ICacheService
    {
        Task SetCacheResponseAsync(string chacheKey, object response, TimeSpan TimeToLive);
        Task<string> GetCacheResponseAsync(string cacheKey);
    }
}
