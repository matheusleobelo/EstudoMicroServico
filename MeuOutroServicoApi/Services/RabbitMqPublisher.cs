using System;
using RabbitMQ.Client;
using RabbitMQ.Client; // Ensure this using is present for IModel
using System.Text;
using System.Text.Json;

namespace MeuOutroServicoApi.Services;

public class RabbitMqPublisher : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchange;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        var cfg = configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory()
        {
            HostName = cfg.GetValue<string>("HostName"),
            Port = cfg.GetValue<int>("Port"),
            UserName = cfg.GetValue<string>("UserName"),
            Password = cfg.GetValue<string>("Password"),
            DispatchConsumersAsync = true
        };

        _exchange = cfg.GetValue<string>("Exchange", "meuservico_exchange");
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // declaramos exchange (idempotente)
        var exchangeType = cfg.GetValue<string>("ExchangeType", "direct");
        _channel.ExchangeDeclare(exchange: _exchange, type: exchangeType, durable: true);
    }

    public void Publish<T>(T message, string routingKey)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = _channel.CreateBasicProperties();
        props.Persistent = true;
        _channel.BasicPublish(exchange: _exchange,
                              routingKey: routingKey,
                              basicProperties: props,
                              body: body);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
