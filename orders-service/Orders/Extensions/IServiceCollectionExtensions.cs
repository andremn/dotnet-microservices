using Orders.Messaging;
using Orders.Messaging.Consumers;
using Orders.Messaging.Consumers.Listeners;
using Orders.Messaging.Messages;
using Orders.Messaging.Producers;
using Orders.Messaging.Producers.Publishers;
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
        services.AddScoped<IOrderProcessingService, OrderProcessingService>();

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IShippingService, ShippingService>();

        var productsServiceAddress = configuration.GetSection("ProductsService:BaseAddress").Value 
            ?? throw new InvalidOperationException("Invalid address for the Products service");

        services.AddRefitClient<IProductService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(productsServiceAddress));

        services.AddSingleton<IRabbitMqService, RabbitMqService>();
        services.AddSingleton<IRabbitMqConsumerService, RabbitMqOrdersConsumerService>();
        services.AddSingleton<IRabbitMqProducerService, RabbitMqProducerService>();

        services.AddTransient<IListener<OrderChangeMessage>, OrderChangeMessageListener>();
        services.AddTransient<IPublisher<OrderChangeMessage>, OrderChangeMessagePublisher>();

        return services;
    }
}
