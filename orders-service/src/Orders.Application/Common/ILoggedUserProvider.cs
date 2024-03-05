using Orders.Application.Dtos;

namespace Orders.Application.Common;

public interface ILoggedUserProvider
{
    LoggedUserDto LoggedUser { get; set; }
}