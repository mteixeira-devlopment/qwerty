using System.Threading.Tasks;
using Identity.API.Responses;
using Identity.API.ViewModels;

namespace Identity.API.Services
{
    public interface IAuthenticationService
    {
        Task CreateUser(NewUser newUser);
    }
}