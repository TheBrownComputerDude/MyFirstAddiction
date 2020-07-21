using api.Dtos;
using MediatR;

namespace api.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}