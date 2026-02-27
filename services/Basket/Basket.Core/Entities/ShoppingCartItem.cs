
namespace Basket.Core.Entities;

public class ShoppingCartItem
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ProductId { get; set; }
    public string ProdcuteName { get; set; }
    public string ImageFile { get; set; }
}
