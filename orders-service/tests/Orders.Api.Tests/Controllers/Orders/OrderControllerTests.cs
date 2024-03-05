using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Orders.Application.Enums;
using Orders.Application.Services.Interfaces;
using Orders.Application.Services.Results;
using Orders.Controllers.Orders;
using Orders.Domain.Dtos;
using Orders.Domain.Enums;

namespace Orders.Api.Tests.Controllers.Orders;

public class OrderControllerTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrdersController _ordersController;

    public OrderControllerTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _ordersController = new OrdersController(_orderServiceMock.Object);
    }

    [Fact]
    public async Task Get_NotEmpty_Returns200OkWithAllAvailableOrders()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var orders = new List<OrderDto>
        {
            new(Id: 1, ProductId: 14, UserId: "user-1", ProductSnapshot: new(Id: 1, "Mouse WiFi", 36.99m), Quantity: 56, Status: OrderStatus.Created, CreatedAt: utcNow.AddDays(-5)),
            new(Id: 2, ProductId: 14, UserId: "user-2", ProductSnapshot: new(Id: 2, "Keyboard Bluetooth", 55.00m), Quantity: 120, Status: OrderStatus.PaymentConfirmed, CreatedAt: utcNow.AddHours(-23)),
            new(Id: 3, ProductId: 15, UserId: "user-1", ProductSnapshot: new(Id: 3, "Hub USB-C", 12.19m), Quantity: 3, Status: OrderStatus.AwaitingPayment, CreatedAt: utcNow.AddMinutes(-15)),
            new(Id: 4, ProductId: 23, UserId: "user-3", ProductSnapshot: new(Id: 4, "Mouse Bluetooth", 25.98m), Quantity: 99, Status: OrderStatus.Shipped, CreatedAt: utcNow)
        };

        _orderServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _ordersController.Get();

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<IEnumerable<OrderDto>>().Should().BeEquivalentTo(orders);
    }

    [Fact]
    public async Task Get_Empty_Returns200OkWithEmptyList()
    {
        // Arrange
        _orderServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);

        // Act
        var result = await _ordersController.Get();

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<IEnumerable<OrderDto>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ExistingId_Returns200OkWithOrderData()
    {
        // Arrange
        var utcNow = DateTime.UtcNow;
        var orders = new List<OrderDto>
        {
            new(Id: 1, ProductId: 14, UserId: "user-1", ProductSnapshot: new(Id: 1, "Mouse WiFi", 36.99m), Quantity: 56, Status: OrderStatus.Created, CreatedAt: utcNow.AddDays(-5)),
            new(Id: 2, ProductId: 14, UserId: "user-2", ProductSnapshot: new(Id: 2, "Keyboard Bluetooth", 55.00m), Quantity: 120, Status: OrderStatus.PaymentConfirmed, CreatedAt: utcNow.AddHours(-23)),
            new(Id: 3, ProductId: 15, UserId: "user-1", ProductSnapshot: new(Id: 3, "Hub USB-C", 12.19m), Quantity: 3, Status: OrderStatus.AwaitingPayment, CreatedAt: utcNow.AddMinutes(-15)),
            new(Id: 4, ProductId: 23, UserId: "user-3", ProductSnapshot: new(Id: 4, "Mouse Bluetooth", 25.98m), Quantity: 99, Status: OrderStatus.Shipped, CreatedAt: utcNow)
        };

        var expectedOrder = orders.Skip(1).First();

        _orderServiceMock.Setup(x => x.GetByIdAsync(expectedOrder.Id))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _ordersController.GetById(expectedOrder.Id);

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<OrderDto>().Should().BeEquivalentTo(expectedOrder);
    }

    [Fact]
    public async Task GetById_NonExistingId_Returns404NotFound()
    {
        // Arrange
        var invalidId = 100;

        _orderServiceMock.Setup(x => x.GetByIdAsync(invalidId))
            .ReturnsAsync((OrderDto?)null);

        // Act
        var result = await _ordersController.GetById(invalidId);

        // Assert
        result.Result.As<NotFoundResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Post_ValidOrder_Returns200OkWithOrderId()
    {
        // Arrange
        var createdId = 12;
        var createOrderRequest = new CreateOrderRequest(ProductId: 15, Quantity: 3);

        var expectedResponse = new CreateOrderResponse(createdId);

        _orderServiceMock.Setup(x => x.CreateAsync(createOrderRequest.ProductId, createOrderRequest.Quantity))
            .ReturnsAsync(new CreateOrderResult(true, createdId, ResultErrorReason.None));

        // Act
        var result = await _ordersController.Post(createOrderRequest);

        // Assert
        var createdAtResult = result.Result.As<CreatedAtActionResult>();

        createdAtResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdAtResult.Value.As<CreateOrderResponse>().Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Post_ProductNotFound_Returns404NotFound()
    {
        // Arrange
        var createOrderRequest = new CreateOrderRequest(ProductId: 15, Quantity: 3);

        _orderServiceMock.Setup(x => x.CreateAsync(createOrderRequest.ProductId, createOrderRequest.Quantity))
            .ReturnsAsync(new CreateOrderResult(false, 0, ResultErrorReason.ProductNotFound));

        // Act
        var result = await _ordersController.Post(createOrderRequest);

        // Assert
        var notFoundResult = result.Result.As<NotFoundResult>();

        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
