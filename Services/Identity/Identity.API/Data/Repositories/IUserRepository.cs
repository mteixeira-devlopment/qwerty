using System.Threading;
using System.Threading.Tasks;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindByNameAsync(string identityName, CancellationToken cancellationToken);
        Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken);
        Task Commit();
    }
}