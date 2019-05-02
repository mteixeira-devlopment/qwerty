using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Notification.API.Hubs
{
    public interface ISignUpHub
    {
        Task Notify();
    }

    public class SignUpHub : Hub<ISignUpHub>
    {
       
    }
}
