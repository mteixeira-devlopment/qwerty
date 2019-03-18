using Bus.Commands;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Identity.API.Configurations
{
    public static class BusConfiguration
    {
        public static IEndpointInstance BusEndpointInstance { get; private set; }

        public static void ConfigureBus(this IServiceCollection service)
        {
            var endpointConfiguration = new EndpointConfiguration("Qwerty.Api.Identity");

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost");
            transport.UsePublisherConfirms(true);
            transport.UseConventionalRoutingTopology();

            var routing = transport.Routing();

            // Validate account route
            routing.RouteToEndpoint(
                assembly: typeof(ValidateAccountCommand).Assembly,
                destination: "Qwerty.Api.Account");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace != null && type.Namespace.EndsWith("Commands"));
            conventions.DefiningEventsAs(type => type.Namespace != null && type.Namespace.EndsWith("Events"));
            conventions.DefiningMessagesAs(type => type.Namespace != null && type.Namespace == "Identity.API.Bus.Messages");

            endpointConfiguration.UseSerialization<XmlSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.EnableInstallers();
         
            endpointConfiguration.UseContainer<ServicesBuilder>(customizations => customizations.ExistingServices(service));

            BusEndpointInstance = Endpoint.Start(endpointConfiguration).Result;
        }
    }
}