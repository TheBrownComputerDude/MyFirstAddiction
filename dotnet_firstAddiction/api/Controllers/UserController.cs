using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Commands;
using api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        public UserController(
            IMediator mediator,
            IConfiguration config)
        {
            this.Mediator = mediator;
            this.Config = config;
        }

        private IMediator Mediator { get; }

        private IConfiguration Config { get; }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(
            string username, string email, string password)
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            var response = await this.Mediator.Send(new LoginCommand()
            {
                Username = username,
                Password = password
            });

            if (response.Response == CommandResponse.Failed)
            {
                return this.Unauthorized();
            }

            var token = this.GenerateJSONWebToken(response.Username);
            return this.Ok(new { token });
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return this.Ok("It works.");
        }
        private string GenerateJSONWebToken(string userInfo)    
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    

            var claims = new[] {    
                new Claim(JwtRegisteredClaimNames.Sub, userInfo)
            };

            var token = new JwtSecurityToken(Config["Jwt:Issuer"],    
              Config["Jwt:Issuer"],    
              claims,    
              expires: DateTime.Now.AddMinutes(120),    
              signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token);    
        }    
    }
}