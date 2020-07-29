using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Commands;
using api.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            IConfiguration config,
            FirstAddictionContext context,
            IMapper mapper)
        {
            this.Mediator = mediator;
            this.Config = config;
            this.Context = context;
            this.Mapper = mapper;
        }

        private IMediator Mediator { get; }

        private IConfiguration Config { get; }

        private FirstAddictionContext Context { get; }

        private IMapper Mapper { get; }

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
        public async Task<IActionResult> Login([FromHeader]string username, [FromHeader]string password)
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

            var token = this.GenerateJSONWebToken(response.UserId);
            return this.Ok(new { token });
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            var currentUser = HttpContext.User;
            var id = currentUser.Claims.FirstOrDefault(c => c.Type == "UserId");
            return this.Ok($"It works. User id is {id}");
        }

        [HttpPost("updateInfo")]
        [Authorize]
        public async Task<IActionResult> Update(IFormCollection collection)
        {
            if (!collection.Keys.Contains("handleName"))
            {
                return this.BadRequest();
            }
            var name = collection["handleName"].FirstOrDefault();
            
            var result = await this.Mediator.Send(new UpdateUserInfoCommand()
                {
                    HandlerName = name
                });
            return this.Ok();
        }

        [HttpGet("getInfo")]
        [Authorize]
        public IActionResult GetInfo()
        {

            var currentUser = HttpContext.User;
            var id = Int32.Parse(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var user = this.Context.User
                .Include(u => u.UserInfo)
                .Where(u => u.Id == id)
                .SingleOrDefault();
            return this.Ok(this.Mapper.Map<Dtos.UserInfo>(user.UserInfo));
        }

        private string GenerateJSONWebToken(int userId)
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
            var claims = new[] {    
                new Claim("UserId", userId.ToString())
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