

using Basket.Core.Entities;

namespace Basket.Core.Repositories;

public interface IBasketRepository
{
    Task<ShppingCart?> GetBasketAsync(string userName);
    Task<ShppingCart> UpdateBasketAsync(ShppingCart basket);
    Task DeleteBasketAsync(string userName);
}
