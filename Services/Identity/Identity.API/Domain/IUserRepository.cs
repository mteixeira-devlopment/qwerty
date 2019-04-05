using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Domain
{
    public interface IUserRepository
    {
        Task<User> FindByNameAsync(string identityName, CancellationToken cancellationToken);
        Task<User> FindAsync(Guid id, CancellationToken cancellationToken);
        Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken);
        void Delete(User user, CancellationToken cancellationToken);
        Task Commit();
    }
}