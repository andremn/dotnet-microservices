using Orders.Domain.Dtos;
using Orders.Domain.Entities;

namespace Orders.Application.Mapping;

public static class ProductSnapshotMapping
{
    public static ProductSnapshotDto ToDto(this ProductSnapshot entity) =>
        new(entity.Id, entity.Name, entity.Price);

    public static ProductSnapshot ToEntity(this ProductSnapshotDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price
        };
}
