using Microsoft.EntityFrameworkCore;
using Orders.Extensions;
using Orders.Model;

namespace Orders.Repositories;

public class OrderRepository(OrdersDbContext dbContext) : IOrderRepository
{
    public async Task<IList<Order>> GetAllByUserAsync(string userId) =>
        await dbContext.Orders
        .AsNoTracking()
        .Where(x => x.UserId == userId)
        .Select(x => x.ToModel())
        .ToListAsync();
}
