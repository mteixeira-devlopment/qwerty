﻿using System.Threading;
using System.Threading.Tasks;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data.Repositories
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

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _identityContext.AddAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task Commit()
        {
            await _identityContext.SaveChangesAsync();
        }
    }
}