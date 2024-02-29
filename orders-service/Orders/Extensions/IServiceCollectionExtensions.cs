using Orders.Repositories;
using Orders.Services;
using Refit;

namespace Orders.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILoggedUserService, LoggedUserService>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();

        var productsServiceAddress = configuration.GetSection("ProductsService:BaseAddress").Value 
            ?? throw new InvalidOperationException("Invalid address for the Products service");

        services.AddRefitClient<IProductService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(productsServiceAddress));

        return services;
    }
}
