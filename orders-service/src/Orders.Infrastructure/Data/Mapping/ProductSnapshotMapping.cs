using Orders.Domain.Models;
using Orders.Infrastructure.Data.Entities;

namespace Orders.Infrastructure.Data.Mapping;

public static class ProductSnapshotMapping
{
    public static ProductSnapshot ToModel(this ProductSnapshotEntity entity) =>
        new(entity.Id, entity.Name, entity.Price);

    public static ProductSnapshotEntity ToEntity(this ProductSnapshot productSnapshot) =>
        new()
        {
            Id = productSnapshot.Id,
            Name = productSnapshot.Name,
            Price = productSnapshot.Price
        };
}
