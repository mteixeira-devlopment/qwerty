using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Notification.API.Hubs
{
    public interface IRequestResponseMessageHub
    {
        Task Notify(string message);
    }

    public class RequestResponseMessageHub : Hub<IRequestResponseMessageHub>
    {
       
    }
}
