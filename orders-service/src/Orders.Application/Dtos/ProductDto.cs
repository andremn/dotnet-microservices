namespace Orders.Application.Dtos;

public record ProductDto(int Id, string Name, string Description, int Quantity, decimal Price);
