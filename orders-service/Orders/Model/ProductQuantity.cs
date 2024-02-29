namespace Orders.Model;

public record UpdateProductQuantityRequest(int Quantity, UpdateProductQuantityOperation Operation);