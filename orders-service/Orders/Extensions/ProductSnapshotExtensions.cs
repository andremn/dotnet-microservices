using Orders.Model;
using Orders.Repositories.Entities;

namespace Orders.Extensions;

public static class ProductSnapshotExtensions
{
    public static ProductSnapshot ToModel(this ProductSnapshotEntity entity) =>
        new(entity.Id, entity.Name, entity.Price);

    public static ProductSnapshotEntity ToEntity(this ProductSnapshot model) =>
        new()
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price
        };
}
