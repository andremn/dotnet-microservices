using Orders.Api.Middlewares;
using Orders.Application.Messaging;

namespace Orders.Extensions;

public static class IApplicationBuilderExtensions
{
    public static void UseRabbitMqConsumers(this IApplicationBuilder applicationBuilder)
    {
        var consumers = applicationBuilder.ApplicationServices.GetServices<IRabbitMqConsumerStarter>();
        var applicationLifetime = applicationBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        applicationLifetime.ApplicationStarted.Register(() =>
        {
            foreach (var consumer in consumers)
            {
                consumer.StartReceivingMessages();
            }
        });

        applicationLifetime.ApplicationStopping.Register(() =>
        {
            foreach (var consumer in consumers)
            {
                consumer.Dispose();
            }
        });
    }

    public static void UseLoggedUserProvider(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ProvideLoggedUserMiddleware>();
    }
}
