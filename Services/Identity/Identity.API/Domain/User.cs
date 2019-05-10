using SharedKernel.Seed;

namespace Identity.API.Domain
{
    public sealed class User : Entity
    {
        public string Username { get; private set; }

        public string PasswordHash { get; private set; }
        public string SecurityStamp { get; private set; }

        private User()
        {
            
        }

        public User(string username)
        {
            Username = username;
        }

        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public void SetSecurityStamp(string securityStamp)
        {
            SecurityStamp = securityStamp;
        }
    }
}
