namespace MassTransit.Message.Broker.Services;

internal interface IRabbitClientService
{
    void SendMessage(object message);
}
