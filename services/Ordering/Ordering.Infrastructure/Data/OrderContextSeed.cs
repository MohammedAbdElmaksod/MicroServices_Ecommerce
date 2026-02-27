
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
    {
        if (!context.Orders.Any())
        {
            await context.Orders.AddRangeAsync(GetOrders());
            await context.SaveChangesAsync();
            logger.LogInformation($"Data seeding about {typeof(OrderContext).Name} is complete");
        }
    }
    public static IEnumerable<Order> GetOrders()
    {
        return new List<Order>
        {
            new()
            {
                UserName = "Mo",
                FirstName = "Mohamed",
                LastName = "Abd-Elmaksod",
                TotalPrice = 700,
                EmailAddress = "mo@test.com",
                AddressLine = "Egypt, Minya",
                Country = "Egypt",
                State = "Minya",
                ZipCode = "777",
                CardName = "Visa",
                CardNumber = "123456789012345",
                Expiration = "01/27",
                Cvv = "432",
                PaymentMethod=1,
                CreatedAt = DateTime.Now,
                CreatedBy = "Mohamed Abd-Elmaksod"
            }
        };
    }
}
