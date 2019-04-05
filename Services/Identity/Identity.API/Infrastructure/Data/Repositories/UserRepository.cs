using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Data.Repositories
{
    public class UserRespository : IUserRepository
    {
        private readonly IdentityContext _identityContext;

        public UserRespository(IdentityContext identityContext)
        {
            _identityContext = identityContext;
            _identityContext.Database.EnsureCreated();
        }

        public async Task<User> FindByNameAsync(string identityName, CancellationToken cancellationToken)
        {
            return await _identityContext.Users
                .FirstOrDefaultAsync(u => u.Username == identityName, cancellationToken);
        }

        public async Task<User> FindAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _identityContext.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _identityContext.AddAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public void Delete(User user, CancellationToken cancellationToken)
        {
            _identityContext.Remove(user);
        }

        public async Task Commit()
        {
            await _identityContext.SaveChangesAsync();
        }
    }
}