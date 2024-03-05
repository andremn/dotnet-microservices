using Orders.Application.Common;
using Orders.Application.Services.Interfaces;
using Orders.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Refit;

namespace Orders.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<ILoggedUserProvider, LoggedUserProvider>();

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderProcessingService, OrderProcessingService>();

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IShippingService, ShippingService>();

        var productsServiceAddress = configuration.GetSection("ProductsService:BaseAddress").Value
            ?? throw new InvalidOperationException("Invalid address for the Products service");

        services.AddRefitClient<IProductService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(productsServiceAddress));

        return services;
    }
}
