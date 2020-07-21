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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Dtos.User>
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

        public Task<Dtos.User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = this.Context.User
                .Where(u => u.Username == request.Username || u.Email == request.Email)
                .SingleOrDefault();
            
            if (response != null)
            {
                return null;
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

            return Task.FromResult(this.Mapper.Map<Dtos.User>(newUser));
        }
    }
}