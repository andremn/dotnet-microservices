using Products.Domain.Models;
using Products.Infrastructure.Data.Entities;

namespace Products.Infrastructure.Mapping;

public static class ProductMapping
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
}
