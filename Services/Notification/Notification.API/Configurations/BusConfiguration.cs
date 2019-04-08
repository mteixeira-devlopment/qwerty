using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Account.API.Configurations
{
    internal static class BusConfiguration
    {
        public static IEndpointInstance BusEndpointInstance { get; private set; }

        public static void ConfigureBus(this IServiceCollection service)
        {
            var endpointConfiguration = new EndpointConfiguration("Qwerty.Api.Notification");

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConfigureTransport();

            var routing = transport.Routing();
            routing.ConfigureRouting();

            var conventions = endpointConfiguration.Conventions();
            conventions.ConfigureConventions();

            endpointConfiguration.ConfigureEndpoint();
         
            endpointConfiguration.UseContainer<ServicesBuilder>(customizations => customizations.ExistingServices(service));

            BusEndpointInstance = Endpoint.Start(endpointConfiguration).Result;
        }

        private static void ConfigureTransport(this TransportExtensions<RabbitMQTransport> transport)
        {
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseConventionalRoutingTopology();
        }

        private static void ConfigureRouting(this RoutingSettings<RabbitMQTransport> routing)
        {

        }

        private static void ConfigureConventions(this ConventionsBuilder conventions)
        {
            conventions.DefiningEventsAs(type => type.Namespace != null && type.Namespace.EndsWith("Events"));
        }

        private static void ConfigureEndpoint(this EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.UseSerialization<XmlSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.SendFailedMessagesTo("Qwerty.Log.Error");
            endpointConfiguration.AuditProcessedMessagesTo("Qwerty.Log.Audit");

            endpointConfiguration.EnableInstallers();
        }
    }
}