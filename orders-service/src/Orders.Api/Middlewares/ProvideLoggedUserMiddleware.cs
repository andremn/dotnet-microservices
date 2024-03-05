using Orders.Api.Services;
using Orders.Application.Common;

namespace Orders.Api.Middlewares;

public class ProvideLoggedUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ILoggedUserService loggedUserService,
        ILoggedUserProvider loggedUserProvider)
    {
        var loggedUser = loggedUserService.GetLoggedUser();

        loggedUserProvider.LoggedUser = loggedUser;

        await next(context);
    }
}
