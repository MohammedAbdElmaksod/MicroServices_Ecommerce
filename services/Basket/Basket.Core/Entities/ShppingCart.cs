
namespace Basket.Core.Entities;

public class ShppingCart
{
    public string UserName { get; set; }

    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    public ShppingCart()
    {
        
    }
    public ShppingCart(string userName)
    {
        UserName = userName;
    }
}
