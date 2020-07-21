using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api.common.Security;
using api.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, CommandResponse>
    {
        public LoginCommandHandler(
            FirstAddictionContext context,
            IPasswordVerifier verifier,
            IMapper mapper)
        {
            this.Context = context;
            this.Verifier = verifier;
            this.Mapper = mapper;
        }

        private FirstAddictionContext Context { get; }

        private IPasswordVerifier Verifier { get; }

        private IMapper Mapper { get; }

        public async Task<CommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await this.Context.User
                .Where(u => u.Username == request.Username)
                .SingleOrDefaultAsync();

            if (user is null)
            {
                return CommandResponse.Failed;
            }

            if (this.Verifier.Validate(request.Password, user.Salt, user.Password))
            {
                return CommandResponse.Success;
            }

            return CommandResponse.Failed;
        }
    }
}