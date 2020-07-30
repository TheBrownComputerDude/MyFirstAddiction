using AutoMapper;

namespace api.Models
{
    public class VideoProfile : Profile
    {
        public VideoProfile()
        {
            this.CreateMap<Video, Dtos.Video>()
                .ConvertUsing(v => new Dtos.Video()
                {
                    Location = v.Location,
                    Id = v.Id
                });
        }
    }
}