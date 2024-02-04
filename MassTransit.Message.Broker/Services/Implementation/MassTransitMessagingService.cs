using System;
using System.Threading.Tasks;

namespace MassTransit.Message.Broker.Services.Implementation;

internal class MassTransitMessagingService(IBus bus) : IMessagingService
{

    private readonly IBus _bus = bus;

    public async Task PublishAsync<T>(object payload) where T : class
    {
        await _bus.Publish<T>(payload);
    }

    public async Task SendAsync<T>(object payload, string queue) where T : class
    {
        var endpointUri = new Uri(_bus.Address, queue);
        var endpoint = await _bus.GetSendEndpoint(endpointUri);
        await endpoint.Send<T>(payload);
    }

}
