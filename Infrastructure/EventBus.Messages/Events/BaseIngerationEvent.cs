
namespace EventBus.Messages.Events;

public class BaseIngerationEvent
{
    public string CorrelationId { get; set; }
    public DateTime CreationDate { get; set; }

    public BaseIngerationEvent()
    {
        CorrelationId = Guid.NewGuid().ToString();
        CreationDate = DateTime.Now;
    }

    public BaseIngerationEvent(Guid correlationId, DateTime creationDate)
    {
        CorrelationId = correlationId.ToString();
        CreationDate = creationDate;
    }
}
