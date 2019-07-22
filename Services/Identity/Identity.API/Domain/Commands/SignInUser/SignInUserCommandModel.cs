using MediatR;
using ServiceSeed.Responses;

namespace Identity.API.Domain.Commands.SignInUser
{
    public sealed class SignInUserCommandModel : IRequest<CommandResponse>
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool RememberCredential { get; private set; }

        public SignInUserCommandModel(string username, string password, bool rememberCredential = false)
        {
            Username = username;
            Password = password;
            RememberCredential = rememberCredential;
        }
    }
}