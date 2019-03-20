using System.Threading.Tasks;
using Identity.API.ViewModels;

namespace Identity.API.Domain
{
    public interface ISignUpService
    {
        Task SignUp(User user, string password);
    }
}