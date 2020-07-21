using api.Dtos;
using MediatR;

namespace api.Commands
{
    public class LoginCommand : IRequest<CommandResponse>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}