using MassTransit.Message.Broker.Models;
using RabbitMQ.Client;
using System.Text.Json;

namespace MassTransit.Message.Broker.Services.Implementation;

internal class RabbitClientService : IRabbitClientService
{
    public void SendMessage(object message)
    {
        var factory = new ConnectionFactory {
            HostName = Connection.HostName,
            VirtualHost = Connection.VirtualHost,
            UserName = Connection.Credentials.Username,
            Password = Connection.Credentials.Password
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: Connection.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = JsonSerializer.SerializeToUtf8Bytes(message);

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: Connection.Queue,
            basicProperties: null,
            body: body);
    }
}
