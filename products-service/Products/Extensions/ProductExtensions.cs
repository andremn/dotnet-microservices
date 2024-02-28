using Products.Controllers.Products;
using Products.Model;
using Products.Repositories.Entities;

namespace Products.Extensions;

public static class ProductExtensions
{
    public static ProductEntity ToEntity(this Product product) =>
        new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Quantity = product.Quantity,
            Price = product.Price
        };

    public static Product ToModel(this ProductEntity entity) =>
        new(entity.Id, entity.Name, entity.Description ?? string.Empty, entity.Quantity, entity.Price);

    public static Product ToModel(this CreateProductRequest request) =>
        new(Id: 0, request.Name, request.Description, request.Quantity, request.Price);
}
