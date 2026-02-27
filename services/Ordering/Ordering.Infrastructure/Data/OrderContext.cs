
using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
        
    }

    public virtual DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach(var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.CreatedBy = "Mohamed"; // TODO:: REPLEACE WITH AUTH SERVER
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.Now;
                    entry.Entity.UpdatedBy = "Mohamed"; // TODO:: REPLEACE WITH AUTH SERVER
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
