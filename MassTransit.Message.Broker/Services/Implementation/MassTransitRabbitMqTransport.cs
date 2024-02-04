using System.Net.Mime;
using MassTransit.Message.Broker.Models;

namespace MassTransit.Message.Broker.Services.Implementation;

internal class MassTransitRabbitMqTransport : IMassTransitTransport
{

    public MassTransitRabbitMqTransport()
    {
        BusControl = ConfigureBus();
        BusControl.Start();
    }

    public IBusControl BusControl { get; }

    static IBusControl ConfigureBus()
    {
        return Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(Connection.Host, host =>
            {
                host.Username(Connection.Credentials.Username);
                host.Password(Connection.Credentials.Password);
            });
        });
    }

}
