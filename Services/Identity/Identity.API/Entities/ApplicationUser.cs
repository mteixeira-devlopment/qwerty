namespace Identity.API.Entities
{
    public sealed class ApplicationUser : Entity<ApplicationUser>
    {
        public string Username { get; private set; }

        public string PasswordHash { get; private set; }
        public string SecurityStamp { get; private set; }

        private ApplicationUser()
        {
            
        }

        public ApplicationUser(string username)
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
