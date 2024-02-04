namespace MassTransit.Message.Broker.Services;

internal interface IMassTransitTransport
{
    IBusControl BusControl { get; }
}
