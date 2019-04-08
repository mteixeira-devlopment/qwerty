using System.Threading.Tasks;

namespace Identity.API.Domain.Services
{
    public interface ISignUpService
    {
        Task<bool> SignUp(User user, string password);
    }
}