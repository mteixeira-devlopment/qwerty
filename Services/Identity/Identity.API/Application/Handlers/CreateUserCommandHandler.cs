using System.Threading;
using System.Threading.Tasks;
using Identity.API.Domain.Commands;
using MediatR;

namespace Identity.API.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandModel, bool>
    {
        public CreateUserCommandHandler()
        {
            
        }

        public async Task<bool> Handle(CreateUserCommandModel request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
