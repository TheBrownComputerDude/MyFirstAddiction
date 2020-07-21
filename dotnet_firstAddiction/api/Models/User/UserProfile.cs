using AutoMapper;

namespace api.Models
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, Dtos.User>()
            .ConvertUsing(
                u => new Dtos.User()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email
                });
        }
    }
}