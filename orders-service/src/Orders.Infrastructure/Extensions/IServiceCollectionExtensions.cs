using Microsoft.Extensions.DependencyInjection;
using Orders.Infrastructure.Data.Repositories;
using Orders.Infrastructure.RabbitMq;
using Orders.Infrastructure.RabbitMq.Producers;
using Orders.Infrastructure.RabbitMq.Consumers.Orders;
using Orders.Application.Messaging.Messages;
using Orders.Domain.Repositories;
using Orders.Application.Messaging;
using Orders.Application.Messaging.Listeners;
using Orders.Application.Messaging.Publishers;
using Orders.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Orders.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();

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

    public static IServiceCollection AddDbContext(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<OrdersDbContext>(options => options.UseSqlite(connectionString));

        return services;
    }
}
