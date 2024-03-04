using Orders.Common;
using Orders.Extensions;
using Orders.Messaging.Messages;
using Orders.Messaging.Producers.Publishers;
using Orders.Model;
using Orders.Repositories;
using Orders.Services.External;
using Orders.Services.Results;

namespace Orders.Services.Orders;

public class OrderService(
    IPublisher<OrderCreatedMessage> orderChangePublisher,
    IProductService productService,
    ILoggedUserService loggedUserService,
    IOrderRepository orderRepository,
    IDateTimeProvider dateTimeProvider
) : IOrderService
{
    private readonly LoggedUser _loggedUser = loggedUserService.GetLoggedUser();

    public async Task<IList<Order>> GetAllAsync()
    {
        var currentUser = loggedUserService.GetLoggedUser();
        var orders = await orderRepository.GetAllByUserAsync(currentUser.Id);

        return orders;
    }

    public async Task<Order?> GetByIdAsync(int id) =>
        await orderRepository.GetByIdAsync(id);

    public async Task<CreateOrderResult> CreateAsync(int productId, int quantity)
    {
        var request = new UpdateProductQuantityRequest(quantity, UpdateProductQuantityOperation.Decrement);
        var updateProductResponse = await productService.UpdateQuantityAsync(productId, request, _loggedUser.Authorization);

        if (updateProductResponse.IsSuccessStatusCode && updateProductResponse.Content is Product product)
        {
            var productSnapshot = new ProductSnapshot(Id: 0, product.Name, product.Price);
            var order = new Order(Id: 0, product.Id, _loggedUser.Id, productSnapshot, quantity, OrderStatus.Created, dateTimeProvider.UtcNow);

            order = await orderRepository.CreateAsync(order);

            orderChangePublisher.Publish(order.ToOrderCreatedMessage());

            return CreateOrderResult.Success(order.Id);
        }

        return CreateOrderResult.ProductNotFound();
    }
}
