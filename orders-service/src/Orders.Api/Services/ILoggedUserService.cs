using Orders.Application.Dtos;

namespace Orders.Api.Services;

public interface ILoggedUserService
{
    LoggedUserDto GetLoggedUser();
}