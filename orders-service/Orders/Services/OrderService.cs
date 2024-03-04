using Orders.Extensions;
using Orders.Messaging.Messages;
using Orders.Messaging.Producers.Publishers;
using Orders.Model;
using Orders.Repositories;
using Orders.Services.Results;

namespace Orders.Services;

public class OrderService(
    IPublisher<OrderCreatedMessage> orderChangePublisher,
    IProductService productService,
    ILoggedUserService loggedUserService,
    IOrderRepository orderRepository
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
            var order = new Order(Id: 0, product.Id, _loggedUser.Id, productSnapshot, quantity, OrderStatus.Created, DateTime.UtcNow);

            order = await orderRepository.CreateAsync(order);

            orderChangePublisher.Publish(order.ToOrderCreatedMessage());

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

        if (existingOrder.ProductSnapshot.Id != order.ProductSnapshot.Id)
        {
            return UpdateOrderResult.CannotUpdateProduct();
        }

        await orderRepository.UpdateAsync(order);

        return UpdateOrderResult.Success(order.Id);
    }
}
