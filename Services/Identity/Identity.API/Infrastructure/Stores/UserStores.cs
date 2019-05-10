using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.API.Domain;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Infrastructure.Stores
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

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Id.ToString());
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Username.ToUpperInvariant());
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.SetPasswordHash(passwordHash);
            return Task.CompletedTask;
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
        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            user.SetSecurityStamp(stamp);
            return Task.CompletedTask;
        }

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.SecurityStamp);
        }
        #endregion
    }
}