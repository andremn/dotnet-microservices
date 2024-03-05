using Microsoft.EntityFrameworkCore;
using Orders.Application.Mapping;
using Orders.Domain.Dtos;
using Orders.Domain.Repositories;

namespace Orders.Infrastructure.Data.Repositories;

public class OrderRepository(OrdersDbContext dbContext) : IOrderRepository
{
    public async Task<IList<OrderDto>> GetAllByUserAsync(string userId) =>
        await dbContext.Orders
        .AsNoTracking()
        .Include(x => x.ProductSnapshot)
        .Where(x => x.UserId == userId)
        .Select(x => x.ToDto())
        .ToListAsync();

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var entity = await dbContext.Orders
        .AsNoTracking()
        .Include(x => x.ProductSnapshot)
        .SingleOrDefaultAsync(x => x.Id == id);

        return entity?.ToDto();
    }

    public async Task<OrderDto> CreateAsync(OrderDto order)
    {
        var entity = order.ToEntity();

        dbContext.Orders.Add(entity);

        await dbContext.SaveChangesAsync();

        return order with { Id = entity.Id, ProductSnapshot = entity.ProductSnapshot.ToDto() };
    }

    public async Task<OrderDto> UpdateAsync(OrderDto order)
    {
        var entity = order.ToEntity();

        dbContext.Orders.Update(entity);

        await dbContext.SaveChangesAsync();

        return order;
    }
}
