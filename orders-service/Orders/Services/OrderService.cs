using Orders.Model;
using Orders.Repositories;

namespace Orders.Services;

public class OrderService(
    IProductService productService,
    ILoggedUserService loggedUserService,
    IOrderRepository orderRepository) : IOrderService
{
    public async Task<IList<DetailedOrder>> GetAllAsync()
    {
        var currentUser = loggedUserService.GetLoggedUser();
        var orders = await orderRepository.GetAllByUserAsync(currentUser.Id);
        var products = await productService.GetAllByIds(orders.Select(x => x.ProductId), currentUser.Authorization);
        var productsById = new Dictionary<int, Product>();
        var detailedOrders = new List<DetailedOrder>(orders.Count);

        foreach (var order in orders)
        {
            if (!productsById.TryGetValue(order.ProductId, out var product))
            {
                product = products.Single(x => x.Id == order.ProductId);
                productsById[product.Id] = product;
            }

            detailedOrders.Add(new DetailedOrder(order.Id, product, order.CreatedAt));
        }

        return detailedOrders;
    }
}
