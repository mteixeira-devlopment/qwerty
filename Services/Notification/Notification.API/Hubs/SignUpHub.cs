using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Notification.API.Hubs
{
    public interface ITypedHubClient
    {
        Task Notify();
    }

    public class SignUpHub : Hub<ITypedHubClient>
    {
       
    }
}
