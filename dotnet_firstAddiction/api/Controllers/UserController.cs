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
        public async Task<IActionResult> CreateUser(
            [FromBody]string username, [FromBody]string email, [FromBody]string password)
        {
            var response = await this.Mediator.Send(new CreateUserCommand()
            {
                Username = username,
                Email = email,
                Password = password
            });
            if (response == CommandResponse.Failed)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody]string username, [FromBody]string password)
        {
            var response = await this.Mediator.Send(new LoginCommand()
            {
                Username = username,
                Password = password
            });

            if (response == CommandResponse.Failed)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}