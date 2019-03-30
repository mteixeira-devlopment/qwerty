using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.API.Data.Repositories;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Stores
{
    public class UserStores : IUserPasswordStore<User>, IUserSecurityStampStore<User>
    {
        private readonly IUserRepository _userRepository;

        public UserStores(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> FindByNameAsync(string identityName, CancellationToken cancellationToken)
        {
            return await _userRepository.FindByNameAsync(identityName, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _userRepository.CreateAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Username);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            _userRepository.Delete(user, cancellationToken);
            return Task.FromResult(IdentityResult.Success);
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _userRepository.FindAsync(new Guid(userId), cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region IUserPasswordStore
        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.SetPasswordHash(passwordHash);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserSecurityStampStore
        public async Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            user.SetSecurityStamp(stamp);
        }

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.SecurityStamp);
        }
        #endregion
    }
}