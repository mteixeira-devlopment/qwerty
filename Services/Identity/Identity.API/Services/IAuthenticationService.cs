using System.Threading.Tasks;
using Identity.API.ViewModels;

namespace Identity.API.Services
{
    public interface IAuthenticationService
    {
        Task SignUpUserAsync(NewUser newUser);
        Task<string> SignInUserAsync(SignInUser signInUser);
    }
}