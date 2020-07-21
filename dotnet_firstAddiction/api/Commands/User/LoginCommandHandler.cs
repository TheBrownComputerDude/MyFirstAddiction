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
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
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

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await this.Context.User
                .Where(u => u.Username == request.Username)
                .SingleOrDefaultAsync();

            var response = new LoginResult()
            {
                Response = CommandResponse.Failed
            };

            if (user is null)
            {
                return response;
            }

            if (this.Verifier.Validate(request.Password, user.Salt, user.Password))
            {
                response.Response = CommandResponse.Success;
                response.Username = user.Username;
                return response;
            }

            return response;
        }
    }
}