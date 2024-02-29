using Orders.Extensions;
using Orders.Messaging.Messages;
using Orders.Messaging.Producers.Publishers;
using Orders.Model;
using Orders.Repositories;
using Orders.Services.Results;

namespace Orders.Services;

public class OrderService(
    IPublisher<OrderChangeMessage> orderChangePublisher,
    IProductService productService,
    ILoggedUserService loggedUserService,
    IOrderRepository orderRepository
) : IOrderService
{
    private readonly LoggedUser _loggedUser = loggedUserService.GetLoggedUser();

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
            return GetDetailedOrderResult.NotFound();
        }

        var response = await productService.GetByIdAsync(order.ProductId, currentUser.Authorization);

        if (response.IsSuccessStatusCode)
        {
            return GetDetailedOrderResult.CreateAsSuccess(CreateDetailedOrder(order, response.Content));
        }

        return GetDetailedOrderResult.ProductNotFound();
    }

    public async Task<CreateOrderResult> CreateAsync(int productId, int quantity)
    {
        var getProductResponse = await productService.GetByIdAsync(productId, _loggedUser.Authorization);

        if (getProductResponse.IsSuccessStatusCode && getProductResponse.Content is Product product)
        {
            var order = new Order(Id: 0, productId, _loggedUser.Id, product.Price, quantity, OrderStatus.Created, DateTime.UtcNow);

            order = await orderRepository.CreateAsync(order);

            orderChangePublisher.Publish(order.ToChangeMessage());

            return CreateOrderResult.Success(order.Id);
        }

        return CreateOrderResult.ProductNotFound();
    }

    public async Task<UpdateOrderResult> UpdateAsync(Order order)
    {
        var existingOrder = await orderRepository.GetByIdAsync(order.Id);

        if (existingOrder is null)
        {
            return UpdateOrderResult.NotFound();
        }

        if (existingOrder.ProductId != order.ProductId)
        {
            return UpdateOrderResult.CannotUpdateProduct();
        }

        await orderRepository.UpdateAsync(order);

        return UpdateOrderResult.Success(order.Id);
    }

    private static DetailedOrder CreateDetailedOrder(Order order, Product product) =>
        new(order.Id, product, order.CreatedAt);
}
