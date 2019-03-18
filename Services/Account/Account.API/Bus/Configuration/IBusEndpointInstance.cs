using NServiceBus;

namespace Account.API.Bus.Configuration
{
    public interface IBusEndpointInstance
    {
        IEndpointInstance EndpointInstance { get; }
    }
}