using Orders.Messaging.Consumers;

namespace Orders.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRabbitMqConsumers(this IApplicationBuilder applicationBuilder)
    {
        var consumers = applicationBuilder.ApplicationServices.GetServices<IRabbitMqConsumerService>();
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

        return applicationBuilder;
    }
}
