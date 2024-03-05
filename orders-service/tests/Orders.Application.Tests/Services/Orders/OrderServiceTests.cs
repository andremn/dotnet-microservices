using FluentAssertions;
using Moq;
using Orders.Application.Common;
using Orders.Application.Dtos;
using Orders.Application.Enums;
using Orders.Application.Messaging.Messages;
using Orders.Application.Messaging.Publishers;
using Orders.Application.Services;
using Orders.Application.Services.Interfaces;
using Orders.Application.Services.Results;
using Orders.Domain.Enums;
using Orders.Domain.Models;
using Orders.Domain.Repositories;
using Refit;
using System.Net;

namespace Orders.Application.Tests.Services.Orders;

public class OrderServiceTests
{
    private readonly Mock<IPublisher<OrderCreatedMessage>> _orderCreatedPublisherMock;
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<ILoggedUserProvider> _loggedUserProviderMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly LoggedUserDto _loggedUser = new("user-1", "User", "Fake", "user@mail.com", "user-token");
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _orderCreatedPublisherMock = new Mock<IPublisher<OrderCreatedMessage>>();
        _productServiceMock = new Mock<IProductService>();
        _loggedUserProviderMock = new Mock<ILoggedUserProvider>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();

        _loggedUserProviderMock.SetupGet(x => x.LoggedUser).Returns(_loggedUser);

        _orderService = new OrderService(
            _orderCreatedPublisherMock.Object,
            _productServiceMock.Object,
            _orderRepositoryMock.Object,
            _dateTimeProviderMock.Object,
            _loggedUserProviderMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_NotEmpty_ReturnsAllAvailableOrders()
    {
        // Arrange
        var loggedUser = new LoggedUserDto("user-1", "User", "Fake", "user@mail.com", string.Empty);
        var utcNow = DateTime.UtcNow;
        var orders = new List<Order>
        {
            new(Id: 1, ProductId: 14, UserId: "user-1", ProductSnapshot: new(Id: 1, "Mouse WiFi", 36.99m), Quantity: 56, Status: OrderStatus.Created, CreatedAt: utcNow.AddDays(-5)),
            new(Id: 2, ProductId: 14, UserId: "user-2", ProductSnapshot: new(Id: 2, "Keyboard Bluetooth", 55.00m), Quantity: 120, Status: OrderStatus.PaymentConfirmed, CreatedAt: utcNow.AddHours(-23)),
            new(Id: 3, ProductId: 15, UserId: "user-1", ProductSnapshot: new(Id: 3, "Hub USB-C", 12.19m), Quantity: 3, Status: OrderStatus.AwaitingPayment, CreatedAt: utcNow.AddMinutes(-15)),
            new(Id: 4, ProductId: 23, UserId: "user-3", ProductSnapshot: new(Id: 4, "Mouse Bluetooth", 25.98m), Quantity: 99, Status: OrderStatus.Shipped, CreatedAt: utcNow)
        };

        _orderRepositoryMock.Setup(x => x.GetAllByUserAsync(loggedUser.Id))
            .ReturnsAsync(orders);

        // Act
        var actualOrders = await _orderService.GetAllAsync();

        // Assert
        actualOrders.Should().BeEquivalentTo(orders);
    }

    [Fact]
    public async Task GetAllAsync_Empty_ReturnsEmptyList()
    {
        // Arrange
        var loggedUser = new LoggedUserDto("user-1", "User", "Fake", "user@mail.com", string.Empty);

        _orderRepositoryMock.Setup(x => x.GetAllByUserAsync(loggedUser.Id))
            .ReturnsAsync([]);

        // Act
        var actualOrders = await _orderService.GetAllAsync();

        // Assert
        actualOrders.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_OrderExists_ReturnsOrderWithMatchingId()
    {
        // Arrange
        var orderId = 23;
        var expectedOrder = new Order(
            Id: 1,
            ProductId: 14,
            UserId: "user-1",
            ProductSnapshot: new(Id: 1, "Mouse WiFi", 36.99m),
            Quantity: 56,
            Status: OrderStatus.Created,
            CreatedAt: DateTime.UtcNow
        );

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(expectedOrder);

        // Act
        var actualOrder = await _orderService.GetByIdAsync(orderId);

        // Assert
        actualOrder.Should().BeEquivalentTo(expectedOrder);
    }

    [Fact]
    public async Task GetByIdAsync_OrderDoesNotExist_ReturnsNull()
    {
        // Arrange
        var orderId = 23;

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync((Order?)null);

        // Act
        var actualOrder = await _orderService.GetByIdAsync(orderId);

        // Assert
        actualOrder.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ProductInStock_ReturnsSuccessWithOrderId()
    {
        // Arrange
        var dateTimeNow = DateTime.UtcNow;
        var productId = 23;
        var quantity = 100;
        var expectedOrderId = 2;
        var updateProductQuantityRequest = new UpdateProductQuantityRequest(quantity, UpdateProductQuantityOperation.Decrement);
        var expectedProduct = new ProductDto(productId, "Keyboard", "RGB", quantity, Price: 15.99m);
        var expectedResult = new CreateOrderResult(true, expectedOrderId, ResultErrorReason.None);
        var expectedOrder = new Order(
            Id: 0,
            ProductId: productId,
            UserId: _loggedUser.Id,
            ProductSnapshot: new(Id: 0, expectedProduct.Name, expectedProduct.Price),
            Quantity: quantity,
            Status: OrderStatus.Created,
            CreatedAt: dateTimeNow);

        _dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(dateTimeNow);

        _productServiceMock.Setup(x => x.UpdateQuantityAsync(productId, updateProductQuantityRequest))
            .ReturnsAsync(new ApiResponse<ProductDto>(new HttpResponseMessage(HttpStatusCode.OK), expectedProduct, new RefitSettings()));

        _orderRepositoryMock.Setup(x => x.CreateAsync(expectedOrder))
            .ReturnsAsync(expectedOrder with { Id = expectedOrderId });

        // Act
        var actualResult = await _orderService.CreateAsync(productId, quantity);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateAsync_ProductInStock_CallsOrderCreatedPublish()
    {
        // Arrange
        var dateTimeNow = DateTime.UtcNow;
        var productId = 23;
        var quantity = 100;
        var expectedOrderId = 2;
        var updateProductQuantityRequest = new UpdateProductQuantityRequest(quantity, UpdateProductQuantityOperation.Decrement);
        var expectedProduct = new ProductDto(productId, "Keyboard", "RGB", quantity, Price: 15.99m);
        var expectedResult = new CreateOrderResult(true, expectedOrderId, ResultErrorReason.None);
        var expectedOrder = new Order(
            Id: 0,
            ProductId: productId,
            UserId: _loggedUser.Id,
            ProductSnapshot: new(Id: 0, expectedProduct.Name, expectedProduct.Price),
            Quantity: quantity,
            Status: OrderStatus.Created,
            CreatedAt: dateTimeNow);

        var expectedOrderCreatedMessage = new OrderCreatedMessage(
            Id: expectedOrderId,
            ProductId: expectedOrder.ProductId,
            UserId: expectedOrder.UserId,
            ProductSnapshot: expectedOrder.ProductSnapshot,
            Quantity: expectedOrder.Quantity,
            CreatedAt: expectedOrder.CreatedAt);

        _dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(dateTimeNow);

        _productServiceMock.Setup(x => x.UpdateQuantityAsync(productId, updateProductQuantityRequest))
            .ReturnsAsync(new ApiResponse<ProductDto>(new HttpResponseMessage(HttpStatusCode.OK), expectedProduct, new RefitSettings()));

        _orderRepositoryMock.Setup(x => x.CreateAsync(expectedOrder))
            .ReturnsAsync(expectedOrder with { Id = expectedOrderId });

        // Act
        var actualResult = await _orderService.CreateAsync(productId, quantity);

        // Assert
        _orderCreatedPublisherMock.Verify(x => x.Publish(expectedOrderCreatedMessage), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ProductOutOfStock_ReturnsProductNotFoundError()
    {
        // Arrange
        var productId = 23;
        var quantity = 100;
        var updateProductQuantityRequest = new UpdateProductQuantityRequest(quantity, UpdateProductQuantityOperation.Decrement);
        var expectedProduct = new ProductDto(productId, "Keyboard", "RGB", quantity, Price: 15.99m);
        var expectedResult = new CreateOrderResult(false, 0, ResultErrorReason.ProductNotFound);

        _productServiceMock.Setup(x => x.UpdateQuantityAsync(productId, updateProductQuantityRequest))
            .ReturnsAsync(new ApiResponse<ProductDto>(new HttpResponseMessage(HttpStatusCode.NotFound), null, new RefitSettings()));

        // Act
        var actualResult = await _orderService.CreateAsync(productId, quantity);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
