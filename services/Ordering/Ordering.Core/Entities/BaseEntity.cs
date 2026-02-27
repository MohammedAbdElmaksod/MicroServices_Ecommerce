

namespace Ordering.Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}
