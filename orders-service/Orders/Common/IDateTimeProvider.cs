
namespace Orders.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}