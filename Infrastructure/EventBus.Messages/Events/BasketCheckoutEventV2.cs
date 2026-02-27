
namespace EventBus.Messages.Events;

public class BasketCheckoutEventV2 : BaseIngerationEvent
{
    public string UserName { get; set; }
    public decimal TotalPrice { get; set; }
}
