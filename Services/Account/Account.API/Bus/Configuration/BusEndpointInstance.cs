using NServiceBus;

namespace Account.API.Bus.Configuration
{
    public sealed class BusEndpointInstance : IBusEndpointInstance
    {
        public IEndpointInstance EndpointInstance { get; }

        public BusEndpointInstance()
        {
            var endpointConfiguration = new EndpointConfiguration("Qwerty.Api.Account");

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseConventionalRoutingTopology();

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace.EndsWith("Commands"));
            conventions.DefiningEventsAs(type => type.Namespace.EndsWith("Events"));
            conventions.DefiningMessagesAs(type => type.Namespace.EndsWith("Commands"));

            endpointConfiguration.UseSerialization<XmlSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.EnableInstallers();

            EndpointInstance = Endpoint.Start(endpointConfiguration).Result;
        }
    }
}