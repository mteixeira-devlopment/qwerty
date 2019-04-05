using Microsoft.AspNetCore.Builder;
using Notification.API.Hubs;

namespace Notification.API.Configurations
{
    internal static class SignalRConfiguration
    {
        public static void UseSignalR(this IApplicationBuilder app)
        {
            app.UseSignalR(routes => { routes.MapHub<SignUpHub>("/jesus"); });
        }
    }
}