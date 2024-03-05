using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.Application.Messaging;
using Orders.Application.Messaging.Configurations;
using Orders.Application.Messaging.Listeners;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Orders.Infrastructure.RabbitMq.Consumers;

public abstract class RabbitMqBaseConsumerStarter<TMessage> : IRabbitMqConsumerStarter
{
    private readonly IConnection _connection;
    private readonly IModel _model;
    private readonly IListener<TMessage> _listener;
    private readonly IOptions<RabbitMqConfiguration> _options;
    private readonly ILogger<RabbitMqBaseConsumerStarter<TMessage>> _logger;

    public RabbitMqBaseConsumerStarter(
        IRabbitMqService rabbitMqService,
        IListener<TMessage> listener,
        IOptions<RabbitMqConfiguration> options,
        ILogger<RabbitMqBaseConsumerStarter<TMessage>> logger)
    {
        _listener = listener;
        _options = options;
        _logger = logger;

        _connection = rabbitMqService.CreateConnection();
        _model = CreateModel();
    }

    protected RabbitMqConfiguration Configuration => _options.Value;

    protected abstract RabbitMqMessageConfiguration MessageConfiguration { get; }

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
                var deserializedMessage = JsonSerializer.Deserialize<TMessage>(message);

                if (deserializedMessage is null)
                {
                    _logger.LogWarning("Could not deserialize message {message} into type '{type}'", deserializedMessage, typeof(TMessage));
                    return;
                }

                await _listener.OnMessageReceived(deserializedMessage);

                _model.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Exception thrown when deserializing JSON message from queue '{Queue}'", MessageConfiguration.Queue);

                // Ack as this is a format error and will never be resolved
                _model.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception e)
            {
                _model.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: true);

                _logger.LogError(e, "Exception thrown when consuming message from queue '{Queue}'", MessageConfiguration.Queue);
            }
        };

        _model.BasicConsume(MessageConfiguration.Queue, false, consumer);
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

    private IModel CreateModel()
    {
        var model = _connection.CreateModel();

        model.QueueDeclare(MessageConfiguration.Queue, durable: true, exclusive: false, autoDelete: false);
        model.ExchangeDeclare(MessageConfiguration.Exchange, ExchangeType.Topic, durable: true, autoDelete: false);

        model.QueueBind(MessageConfiguration.Queue, MessageConfiguration.Exchange, MessageConfiguration.RoutingKey);

        return model;
    }
}
