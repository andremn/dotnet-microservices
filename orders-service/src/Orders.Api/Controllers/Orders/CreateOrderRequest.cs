namespace Orders.Controllers.Orders;

public record CreateOrderRequest(int ProductId, int Quantity);