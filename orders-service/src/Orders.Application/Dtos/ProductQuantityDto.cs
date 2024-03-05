using Orders.Application.Enums;

namespace Orders.Application.Dtos;

public record UpdateProductQuantityRequest(int Quantity, UpdateProductQuantityOperation Operation);