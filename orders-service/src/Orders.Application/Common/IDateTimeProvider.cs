
namespace Orders.Application.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}