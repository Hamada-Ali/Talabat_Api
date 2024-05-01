using Microsoft.EntityFrameworkCore.ValueGeneration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer redis)
        {
            // to connect to  redis database
            _database = redis.GetDatabase();
        }

        public async Task<string> GetCacheResponseAsync(string cacheKey)
        {
            var cachedResposne = await _database.StringGetAsync(cacheKey);

            if(cacheKey is null)
            {
                return null;
            }

            return cachedResposne;
        }

        public async Task SetCacheResponseAsync(string chacheKey, object response, TimeSpan TimeToLive)
        {
            if (response is null)
                return;

            // optional CamelCase Formate
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var seralizedResponse = JsonSerializer.Serialize(response, options);

            await _database.StringSetAsync(chacheKey, seralizedResponse, TimeToLive);
        }
    }
}
