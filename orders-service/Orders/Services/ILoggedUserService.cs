using Orders.Model;

namespace Orders.Services;

public interface ILoggedUserService
{
    LoggedUser GetLoggedUser();
}