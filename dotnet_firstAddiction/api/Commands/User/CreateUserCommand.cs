using api.Dtos;
using MediatR;

namespace api.Commands
{
    public class CreateUserCommand : IRequest<CommandResponse>
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}