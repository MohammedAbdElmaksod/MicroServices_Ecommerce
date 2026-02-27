using Basket.Core.Entities;

namespace Basket.Application.Responses;

public class ShoppingCartResponse
{
    public string UserName { get; set; }

    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    public ShoppingCartResponse()
    {

    }
    public ShoppingCartResponse(string userName)
    {
        UserName = userName;
    }
    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
}
