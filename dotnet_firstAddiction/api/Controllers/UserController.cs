using System.Threading.Tasks;
using api.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        public UserController(
            IMediator mediator)
        {
            this.Mediator = mediator;
        }

        private IMediator Mediator { get; }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(string username, string email, string password)
        {
            var response = await this.Mediator.Send(new CreateUserCommand()
            {
                Username = username,
                Email = email,
                Password = password
            });
            if (response is null)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}