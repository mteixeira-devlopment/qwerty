using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.API.Data.Repositories;
using Identity.API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Stores
{
    public class UserStores : IUserPasswordStore<ApplicationUser>, IUserSecurityStampStore<ApplicationUser>
    {
        private readonly IUserRepository _userRepository;

        public UserStores(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApplicationUser> FindByNameAsync(string identityName, CancellationToken cancellationToken)
        {
            return await _userRepository.FindByNameAsync(identityName, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _userRepository.CreateAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Username);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region IUserPasswordStore
        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.SetPasswordHash(passwordHash);
        }

        public async Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserSecurityStampStore
        public async Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
        {
            user.SetSecurityStamp(stamp);
        }

        public async Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.SecurityStamp);
        }
        #endregion
    }
}