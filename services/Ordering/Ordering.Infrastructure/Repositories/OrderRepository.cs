

using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{

    public OrderRepository(OrderContext _context) : base(_context)
    {
 
    }
    public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
    {
        return await _context.Orders.Where(u => u.UserName == userName).ToListAsync();
    }
}
