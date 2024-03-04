using Orders.Common;
using Orders.Messaging;
using Orders.Messaging.Consumers;
using Orders.Messaging.Consumers.Listeners;
using Orders.Messaging.Messages;
using Orders.Messaging.Producers;
using Orders.Messaging.Producers.Publishers;
using Orders.Repositories;
using Orders.Services;
using Orders.Services.External;
using Orders.Services.Orders;
using Refit;

namespace Orders.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

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
        services.AddSingleton<IRabbitMqProducerService, RabbitMqProducerService>();
        services.AddSingleton<IRabbitMqConsumerStarter, RabbitMqOrderCreatedConsumerStarter>();
        services.AddSingleton<IRabbitMqConsumerStarter, RabbitMqOrderPaymentStatusChangedConsumerStarter>();
        services.AddSingleton<IRabbitMqConsumerStarter, RabbitMqOrderPaymentRequestConsumerStarter>();
        services.AddSingleton<IRabbitMqConsumerStarter, RabbitMqOrderShippingStatusChangedConsumerStarter>();
        services.AddSingleton<IRabbitMqConsumerStarter, RabbitMqOrderShippingRequestConsumerStarter>();

        services.AddTransient<IListener<OrderCreatedMessage>, OrderCreatedMessageListener>();
        services.AddTransient<IListener<OrderPaymentStatusChangedMessage>, OrderPaymentStatusChangedListener>();
        services.AddTransient<IListener<OrderPaymentRequestMessage>, OrderPaymentRequestListener>();
        services.AddTransient<IListener<OrderShippingStatusChangedMessage>, OrderShippingStatusChangedListener>();
        services.AddTransient<IListener<OrderShippingRequestMessage>, OrderShippingRequestListener>();

        services.AddScoped<IPublisher<OrderCreatedMessage>, OrderCreatedPublisher>();
        services.AddScoped<IPublisher<OrderPaymentRequestMessage>, OrderPaymentRequestPublisher>();
        services.AddScoped<IPublisher<OrderShippingRequestMessage>, OrderShippingRequestPublisher>();
        services.AddScoped<IPublisher<OrderPaymentStatusChangedMessage>, OrderPaymentStatusChangedPublisher>();
        services.AddScoped<IPublisher<OrderShippingStatusChangedMessage>, OrderShippingStatusChangedPublisher>();

        return services;
    }
}
