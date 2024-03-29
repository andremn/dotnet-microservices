﻿using Microsoft.EntityFrameworkCore;
using Orders.Domain.Models;
using Orders.Domain.Repositories;
using Orders.Infrastructure.Data.Mapping;

namespace Orders.Infrastructure.Data.Repositories;

public class OrderRepository(OrdersDbContext dbContext) : IOrderRepository
{
    public async Task<IList<Order>> GetAllByUserAsync(string userId) =>
        await dbContext.Orders
        .AsNoTracking()
        .Include(x => x.ProductSnapshot)
        .Where(x => x.UserId == userId)
        .Select(x => x.ToModel())
        .ToListAsync();

    public async Task<Order?> GetByIdAsync(int id)
    {
        var entity = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.ProductSnapshot)
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity?.ToModel();
    }

    public async Task<Order> CreateAsync(Order order)
    {
        var entity = order.ToEntity();

        dbContext.Orders.Add(entity);

        await dbContext.SaveChangesAsync();

        return order with { Id = entity.Id, ProductSnapshot = entity.ProductSnapshot.ToModel() };
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        var entity = order.ToEntity();

        dbContext.Orders.Update(entity);

        await dbContext.SaveChangesAsync();

        return order;
    }
}
