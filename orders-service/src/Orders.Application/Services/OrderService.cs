using Orders.Application.Common;
using Orders.Application.Dtos;
using Orders.Application.Enums;
using Orders.Application.Mapping;
using Orders.Application.Messaging.Messages;
using Orders.Application.Messaging.Publishers;
using Orders.Application.Services.Interfaces;
using Orders.Application.Services.Results;
using Orders.Domain.Enums;
using Orders.Domain.Models;
using Orders.Domain.Repositories;

namespace Orders.Application.Services;

public class OrderService(
    IPublisher<OrderCreatedMessage> orderChangePublisher,
    IProductService productService,
    IOrderRepository orderRepository,
    IDateTimeProvider dateTimeProvider,
    ILoggedUserProvider loggedUserProvider
) : IOrderService
{
    public async Task<IList<Order>> GetAllAsync()
    {
        var orders = await orderRepository.GetAllByUserAsync(loggedUserProvider.LoggedUser.Id);

        return orders;
    }

    public async Task<Order?> GetByIdAsync(int id) =>
        await orderRepository.GetByIdAsync(id);

    public async Task<CreateOrderResult> CreateAsync(int productId, int quantity)
    {
        var request = new UpdateProductQuantityRequest(quantity, UpdateProductQuantityOperation.Decrement);
        var updateProductResponse = await productService.UpdateQuantityAsync(productId, request);

        if (updateProductResponse.IsSuccessStatusCode && updateProductResponse.Content is ProductDto product)
        {
            var productSnapshot = new ProductSnapshot(Id: 0, product.Name, product.Price);
            var order = new Order(
                Id: 0,
                product.Id,
                loggedUserProvider.LoggedUser.Id,
                productSnapshot,
                quantity,
                OrderStatus.Created,
                dateTimeProvider.UtcNow);

            order = await orderRepository.CreateAsync(order);

            orderChangePublisher.Publish(order.ToOrderCreatedMessage());

            return CreateOrderResult.Success(order.Id);
        }

        return CreateOrderResult.ProductNotFound();
    }
}
