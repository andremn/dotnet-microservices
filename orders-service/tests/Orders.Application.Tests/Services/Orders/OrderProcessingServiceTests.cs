using Microsoft.Extensions.Logging;
using Moq;
using Orders.Application.Enums;
using Orders.Application.Services;
using Orders.Application.Services.Interfaces;
using Orders.Domain.Enums;
using Orders.Domain.Models;
using Orders.Domain.Repositories;

namespace Orders.Application.Tests.Services.Orders;

public class OrderProcessingServiceTests
{
    private readonly Mock<IPaymentService> _paymentServiceMock;
    private readonly Mock<IShippingService> _shippingServiceMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ILogger<OrderProcessingService>> _loggerMock;
    private readonly OrderProcessingService _orderProcessingService;

    public OrderProcessingServiceTests()
    {
        _paymentServiceMock = new Mock<IPaymentService>();
        _shippingServiceMock = new Mock<IShippingService>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _loggerMock = new Mock<ILogger<OrderProcessingService>>();

        _orderProcessingService = new OrderProcessingService(
            _paymentServiceMock.Object,
            _shippingServiceMock.Object,
            _orderRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task HandleOrderCreatedAsync_CallsPaymentApprovalRequest()
    {
        // Arrange
        var order = new Order(
            Id: 33,
            ProductId: 132,
            UserId: "user-1",
            ProductSnapshot: new(Id: 0, "Keyboard", 33.99m),
            Quantity: 11,
            Status: OrderStatus.Created,
            CreatedAt: DateTime.UtcNow);

        // Act
        await _orderProcessingService.HandleOrderCreatedAsync(order);

        // Assert
        _paymentServiceMock.Verify(x => x.SendApprovalRequestAsync(order), Times.Once);
    }

    [Fact]
    public async Task HandleOrderCreatedAsync_UpdatesStatusToAwaitingPayment()
    {
        // Arrange
        var order = new Order(
            Id: 110,
            ProductId: 132,
            UserId: "user-1",
            ProductSnapshot: new(Id: 0, "Keyboard", 33.99m),
            Quantity: 11,
            Status: OrderStatus.Created,
            CreatedAt: DateTime.UtcNow);

        var expectedOrder = order with { Status = OrderStatus.AwaitingPayment };

        // Act
        await _orderProcessingService.HandleOrderCreatedAsync(order);

        // Assert
        _orderRepositoryMock.Verify(x => x.UpdateAsync(expectedOrder), Times.Once);
    }

    [Theory]
    [InlineData(OrderPaymentStatus.AwaitingApproval)]
    [InlineData(OrderPaymentStatus.Approved)]
    [InlineData(OrderPaymentStatus.Denied)]
    [InlineData((OrderPaymentStatus)999)]
    public async Task HandlePaymentStatusChangedAsync_OrderExists_UpdatesStatus(OrderPaymentStatus orderPaymentStatus)
    {
        // Arrange
        var order = new Order(
            Id: 10,
            ProductId: 132,
            UserId: "user-1",
            ProductSnapshot: new(Id: 0, "Keyboard", 33.99m),
            Quantity: 11,
            Status: OrderStatus.Created,
            CreatedAt: DateTime.UtcNow);

        var expectedOrder = order with
        {
            Status = orderPaymentStatus switch
            {
                OrderPaymentStatus.AwaitingApproval => OrderStatus.AwaitingPayment,
                OrderPaymentStatus.Approved => OrderStatus.PaymentConfirmed,
                OrderPaymentStatus.Denied => OrderStatus.PaymentDenied,
                _ => order.Status
            }
        };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id))
            .ReturnsAsync(order);

        // Act
        await _orderProcessingService.HandlePaymentStatusChangedAsync(order.Id, orderPaymentStatus);

        // Assert
        _orderRepositoryMock.Verify(x => x.UpdateAsync(expectedOrder), Times.Once);
    }

    [Theory]
    [InlineData(OrderPaymentStatus.AwaitingApproval)]
    [InlineData(OrderPaymentStatus.Approved)]
    [InlineData(OrderPaymentStatus.Denied)]
    [InlineData((OrderPaymentStatus)999)]
    public async Task HandlePaymentStatusChangedAsync_OrderExists_CallsRequestOrderShipping(OrderPaymentStatus orderPaymentStatus)
    {
        // Arrange
        var order = new Order(
            Id: 10,
            ProductId: 132,
            UserId: "user-1",
            ProductSnapshot: new(Id: 0, "Keyboard", 33.99m),
            Quantity: 11,
            Status: OrderStatus.Created,
            CreatedAt: DateTime.UtcNow);

        var expectedOrder = order with
        {
            Status = orderPaymentStatus switch
            {
                OrderPaymentStatus.AwaitingApproval => OrderStatus.AwaitingPayment,
                OrderPaymentStatus.Approved => OrderStatus.PaymentConfirmed,
                OrderPaymentStatus.Denied => OrderStatus.PaymentDenied,
                _ => order.Status
            }
        };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id))
            .ReturnsAsync(order);

        // Act
        await _orderProcessingService.HandlePaymentStatusChangedAsync(order.Id, orderPaymentStatus);

        // Assert
        _shippingServiceMock.Verify(x => x.RequestOrderShippingAsync(expectedOrder), Times.Once);
    }

    [Theory]
    [InlineData(OrderPaymentStatus.AwaitingApproval)]
    [InlineData(OrderPaymentStatus.Approved)]
    [InlineData(OrderPaymentStatus.Denied)]
    [InlineData((OrderPaymentStatus)999)]
    public async Task HandlePaymentStatusChangedAsync_OrderDoesNotExist_DoNotUpdateStatus(OrderPaymentStatus orderPaymentStatus)
    {
        // Arrange
        var order = new Order(
            Id: 10,
            ProductId: 132,
            UserId: "user-1",
            ProductSnapshot: new(Id: 0, "Keyboard", 33.99m),
            Quantity: 11,
            Status: OrderStatus.Created,
            CreatedAt: DateTime.UtcNow);

        var expectedOrder = order with
        {
            Status = orderPaymentStatus switch
            {
                OrderPaymentStatus.AwaitingApproval => OrderStatus.AwaitingPayment,
                OrderPaymentStatus.Approved => OrderStatus.PaymentConfirmed,
                OrderPaymentStatus.Denied => OrderStatus.PaymentDenied,
                _ => order.Status
            }
        };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id))
            .ReturnsAsync((Order?)null);

        // Act
        await _orderProcessingService.HandlePaymentStatusChangedAsync(order.Id, orderPaymentStatus);

        // Assert
        _orderRepositoryMock.Verify(x => x.UpdateAsync(expectedOrder), Times.Never);

        _loggerMock.Verify(x =>
            x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals($"Payment status changed, but order with id '{order.Id}' was not found", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Theory]
    [InlineData(OrderShippingStatus.AwaitingCollect)]
    [InlineData(OrderShippingStatus.Collected)]
    [InlineData(OrderShippingStatus.EnRoute)]
    [InlineData(OrderShippingStatus.Delivered)]
    [InlineData(OrderShippingStatus.NotDelivered)]
    [InlineData((OrderShippingStatus)999)]
    public async Task HandleShippingStatusChangedAsync_OrderExists_UpdatesStatus(OrderShippingStatus orderShippingStatus)
    {
        // Arrange
        var order = new Order(
            Id: 10,
            ProductId: 132,
            UserId: "user-1",
            ProductSnapshot: new(Id: 0, "Keyboard", 33.99m),
            Quantity: 11,
            Status: OrderStatus.Created,
            CreatedAt: DateTime.UtcNow);

        var expectedOrder = order with
        {
            Status = orderShippingStatus switch
            {
                OrderShippingStatus.AwaitingCollect => OrderStatus.AwaitingShipping,
                OrderShippingStatus.Collected => OrderStatus.Shipped,
                OrderShippingStatus.EnRoute => OrderStatus.Shipped,
                OrderShippingStatus.Delivered => OrderStatus.Finished,
                OrderShippingStatus.NotDelivered => OrderStatus.DeliveryFailed,
                _ => order.Status
            }
        };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id))
            .ReturnsAsync(order);

        // Act
        await _orderProcessingService.HandleShippingStatusChangedAsync(order.Id, orderShippingStatus);

        // Assert
        _orderRepositoryMock.Verify(x => x.UpdateAsync(expectedOrder), Times.Once);
    }

    [Theory]
    [InlineData(OrderShippingStatus.AwaitingCollect)]
    [InlineData(OrderShippingStatus.Collected)]
    [InlineData(OrderShippingStatus.EnRoute)]
    [InlineData(OrderShippingStatus.Delivered)]
    [InlineData(OrderShippingStatus.NotDelivered)]
    [InlineData((OrderShippingStatus)999)]
    public async Task HandleShippingStatusChangedAsync_OrderDoesNotExist_DoNotUpdateStatus(OrderShippingStatus orderShippingStatus)
    {
        // Arrange
        var order = new Order(
            Id: 10,
            ProductId: 132,
            UserId: "user-1",
            ProductSnapshot: new(Id: 0, "Keyboard", 33.99m),
            Quantity: 11,
            Status: OrderStatus.Created,
            CreatedAt: DateTime.UtcNow);

        var expectedOrder = order with
        {
            Status = orderShippingStatus switch
            {
                OrderShippingStatus.AwaitingCollect => OrderStatus.AwaitingShipping,
                OrderShippingStatus.Collected => OrderStatus.Shipped,
                OrderShippingStatus.EnRoute => OrderStatus.Shipped,
                OrderShippingStatus.Delivered => OrderStatus.Finished,
                OrderShippingStatus.NotDelivered => OrderStatus.DeliveryFailed,
                _ => order.Status
            }
        };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id))
            .ReturnsAsync((Order?)null);

        // Act
        await _orderProcessingService.HandleShippingStatusChangedAsync(order.Id, orderShippingStatus);

        // Assert
        _orderRepositoryMock.Verify(x => x.UpdateAsync(expectedOrder), Times.Never);

        _loggerMock.Verify(x =>
            x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals($"Shipping was started, but order with id '{order.Id}' was not found", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
