using Orders.Model;
using Orders.Repositories;
using Orders.Services.Results;

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
        var products = await productService.GetAllByIdsAsync(orders.Select(x => x.ProductId), currentUser.Authorization);
        var productsById = new Dictionary<int, Product>();
        var detailedOrders = new List<DetailedOrder>(orders.Count);

        foreach (var order in orders)
        {
            if (!productsById.TryGetValue(order.ProductId, out var product))
            {
                product = products.Single(x => x.Id == order.ProductId);
                productsById[product.Id] = product;
            }

            detailedOrders.Add(CreateDetailedOrder(order, product));
        }

        return detailedOrders;
    }

    public async Task<GetDetailedOrderResult> GetByIdAsync(int id)
    {
        var order = await orderRepository.GetByIdAsync(id);
        var currentUser = loggedUserService.GetLoggedUser();

        if (order is null)
        {
            return GetDetailedOrderResult.FromNotFoundError();
        }

        var response = await productService.GetByIdAsync(order.ProductId, currentUser.Authorization);

        if (response.IsSuccessStatusCode)
        {
            return GetDetailedOrderResult.FromSuccess(CreateDetailedOrder(order, response.Content));
        }

        return GetDetailedOrderResult.FromProductNotFoundError();
    }

    public async Task<CreateOrderResult> CreateAsync(int productId)
    {
        var currentUser = loggedUserService.GetLoggedUser();
        var response = await productService.GetByIdAsync(productId, currentUser.Authorization);

        if (response.IsSuccessStatusCode)
        {
            var product = response.Content;
            var order = new Order(Id: 0, productId, currentUser.Id, product.Price, OrderStatus.Created, DateTime.UtcNow);

            order = await orderRepository.CreateAsync(order);

            return CreateOrderResult.FromSuccess(order.Id);
        }
        
        return CreateOrderResult.FromNotFoundError();
    }

    private static DetailedOrder CreateDetailedOrder(Order order, Product product) =>
        new(order.Id, product, order.CreatedAt);
}
