

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.Data;

public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
{
    public OrderContext CreateDbContext(string[] args)
    {
        var builderOptions = new DbContextOptionsBuilder<OrderContext>();
        builderOptions.UseSqlServer("Server=localhost;Database=OrderDb;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=True;");
        return new OrderContext(builderOptions.Options);
    }
}
