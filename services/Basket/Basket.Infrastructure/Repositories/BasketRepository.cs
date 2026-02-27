
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<ShppingCart?> GetBasketAsync(string userName)
    {
        var result = await _redisCache.GetStringAsync(userName);
        if (string.IsNullOrEmpty(result))
        {
            return null;
        }
        return JsonConvert.DeserializeObject<ShppingCart>(result);
    }

    public async Task<ShppingCart> UpdateBasketAsync(ShppingCart basket)
    {
        var result = await _redisCache.GetStringAsync(basket.UserName);
        if(result != null)
            return await GetBasketAsync(basket.UserName);

        await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
        return await GetBasketAsync(basket.UserName);
    }

    public async Task DeleteBasketAsync(string userName)
    {
        var result = await _redisCache.GetStringAsync(userName);
        if (result != null)
             await _redisCache.RemoveAsync(userName);
    }
}
