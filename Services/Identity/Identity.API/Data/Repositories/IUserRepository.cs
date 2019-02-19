using System.Threading;
using System.Threading.Tasks;
using Identity.API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Data.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindByNameAsync(string identityName, CancellationToken cancellationToken);
        Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken);
        Task Commit();
    }
}