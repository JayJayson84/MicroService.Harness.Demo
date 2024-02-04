using System.Threading.Tasks;

namespace MassTransit.Message.Broker.Services;

internal interface IMessagingService
{
    Task PublishAsync<T>(object payload) where T : class;
    Task SendAsync<T>(object payload, string queue) where T : class;
}
