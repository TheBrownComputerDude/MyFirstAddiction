namespace api.Commands
{
    using api.Dtos;
    using MediatR;

    public class UpdateUserInfoCommand : IRequest<UserInfo>
    {
        public string HandlerName { get; set; }
    }
}