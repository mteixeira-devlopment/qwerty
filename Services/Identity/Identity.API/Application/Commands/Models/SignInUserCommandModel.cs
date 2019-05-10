using MediatR;
using SharedKernel.Responses;

namespace Identity.API.Application.Commands.Models
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