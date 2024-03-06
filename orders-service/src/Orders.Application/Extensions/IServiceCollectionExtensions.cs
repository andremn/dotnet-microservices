using Orders.Application.Common;
using Orders.Application.Services.Interfaces;
using Orders.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Refit;

namespace Orders.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<ILoggedUserProvider, LoggedUserProvider>();

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderProcessingService, OrderProcessingService>();

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IShippingService, ShippingService>();

        return services;
    }

    public static IServiceCollection AddProductsServiceRefitClient(this IServiceCollection services, Action<IHttpClientBuilder> httpClientOptions)
    {
        var httpClientBuilder = services.AddRefitClient<IProductService>();

        httpClientOptions(httpClientBuilder);

        return services;
    }
}
