using Microsoft.Extensions.Options;
using Orders.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static Orders.Configurations.RabbitMqConfig;

namespace Orders.Messaging.Consumers;

public abstract class RabbitMqConsumerService : IRabbitMqConsumerService
{
    private readonly IModel _model;
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqConsumerService> _logger;

    public RabbitMqConsumerService(
        IRabbitMqService rabbitMqService,
        IOptions<RabbitMqConfig> options,
        ILogger<RabbitMqConsumerService> logger)
    {
        Config = options.Value;

        _connection = rabbitMqService.CreateConnection();
        _model = CreateModel(_connection);
        _logger = logger;
    }

    protected RabbitMqConfig Config { get; }

    protected abstract RabbitMqConsumerConfig ConsumerConfig { get; }

    public void StartReceivingMessages()
    {
        var consumer = new AsyncEventingBasicConsumer(_model);

        consumer.Received += async (channel, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogDebug("Received message: {message}", message);

            try
            {
                await ProcessAsync(message);
                _model.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception e)
            {
                _model.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: true);

                _logger.LogError(e, "Exception thrown when consuming message from queue '{Queue}'", ConsumerConfig.Queue);
            }
        };

        _model.BasicConsume(ConsumerConfig.Queue, false, consumer);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_model.IsOpen)
            {
                _model.Close();
            }

            if (_connection.IsOpen)
            {
                _connection.Close();
            }
        }
    }

    protected abstract IModel CreateModel(IConnection connection);

    protected abstract Task ProcessAsync(string message);
}
