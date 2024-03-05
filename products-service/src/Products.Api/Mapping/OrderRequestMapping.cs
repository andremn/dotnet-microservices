using Products.Api.Controllers.Products;
using Products.Domain.Models;

namespace Products.Api.Mapping;

public static class OrderRequestMapping
{
    public static Product ToDto(this CreateProductRequest request) =>
        new(Id: 0, request.Name, request.Description, request.Quantity, request.Price);

    public static Product ToDto(this UpdateProductRequest request, int id) =>
        new(Id: id, request.Name, request.Description, request.Quantity, request.Price);
}
