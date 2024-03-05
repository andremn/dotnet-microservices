using Orders.Api.Services;

namespace Orders.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ILoggedUserService, LoggedUserService>();

        return services;
    }
}
