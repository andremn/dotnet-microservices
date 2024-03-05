using Microsoft.Extensions.DependencyInjection;
using Users.Application.Services;
using Users.Application.Services.Interfaces;

namespace Users.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
