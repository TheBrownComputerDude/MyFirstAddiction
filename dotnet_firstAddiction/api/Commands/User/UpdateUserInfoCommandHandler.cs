namespace api.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using api.Models;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, Dtos.UserInfo>
    {
        public UpdateUserInfoCommandHandler(
            IHttpContextAccessor accessor,
            FirstAddictionContext context,
            IMapper mapper)
        {
            this.Accessor = accessor;
            this.Context = context;
            this.Mapper = mapper;
        }

        private IHttpContextAccessor Accessor { get; }

        private FirstAddictionContext Context { get; }

        private IMapper Mapper { get; }

        public Task<Dtos.UserInfo> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            var userId = Accessor.HttpContext.User.Claims
                .FirstOrDefault(u => u.Type == "UserId");
            if (userId is null) {
                throw new ArgumentNullException("User Id is null");
            }

            var user = this.Context.User
                .Include(u => u.UserInfo)
                .Where(u => u.Id == Int32.Parse(userId.Value))
                .SingleOrDefault();

            var newInfo = new UserInfo()
            {
                HandleName = request.HandlerName
            };
            user.UserInfo = newInfo;

            this.Context.Update(user);
            this.Context.SaveChanges();
            return Task.FromResult(this.Mapper.Map<Dtos.UserInfo>(newInfo));
        }
    }
}