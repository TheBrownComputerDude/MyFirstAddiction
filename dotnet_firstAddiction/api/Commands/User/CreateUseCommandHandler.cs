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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommandResponse>
    {
        public CreateUserCommandHandler(
            IMapper mapper,
            IPasswordVerifier verifier,
            FirstAddictionContext context)
        {
            this.Mapper = mapper;
            this.Verifier = verifier;
            this.Context = context;
        }

        private IMapper Mapper { get; }

        private IPasswordVerifier Verifier { get; }

        private FirstAddictionContext Context { get; }

        public async Task<CommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = await this.Context.User
                .Where(u => u.Username == request.Username || u.Email == request.Email)
                .SingleOrDefaultAsync();
            
            if (response != null)
            {
                return CommandResponse.Failed;
            }

            var passResponse = this.Verifier.HashPassword(request.Password);

            var newUser = new User()
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = passResponse.Hash,
                    Salt = passResponse.Salt
                };

            this.Context.Add(newUser);
            this.Context.SaveChanges();

            return CommandResponse.Success;
        }
    }
}