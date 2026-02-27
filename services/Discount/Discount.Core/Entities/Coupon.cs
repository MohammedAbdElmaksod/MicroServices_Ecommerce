
namespace Discount.Core.Entities;

public class Coupon
{
    public int Id { get; set; }
    public string ProductName { get; set; } // should be uniuqe
    public string Description { get; set; }
    public int Amount { get; set; }
}
