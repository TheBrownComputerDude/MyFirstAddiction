namespace api.Commands
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using api.Models;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class AddVideoCommandHandler : IRequestHandler<AddVideoCommand, Dtos.Video>
    {
        public AddVideoCommandHandler(
            FirstAddictionContext context,
            ILogger<AddVideoCommandHandler> logger,
            IHttpContextAccessor accessor,
            IMapper mapper)
        {
            this.Context = context;
            this.Logger = logger;
            this.Accessor = accessor;
            this.Mapper = mapper;
        }

        private FirstAddictionContext Context { get; }

        private ILogger<AddVideoCommandHandler> Logger { get; }

        private IHttpContextAccessor Accessor { get; }

        private IMapper Mapper { get; }

        public Task<Dtos.Video> Handle(AddVideoCommand request, CancellationToken cancellationToken)
        {
            var userId = Accessor.HttpContext.User.Claims
                .FirstOrDefault(u => u.Type == "UserId");
            if (userId is null) {
                throw new ArgumentNullException("User Id is null");
            }

            string thumbnailPath = Path.Combine(
                Path.GetDirectoryName(request.Location),
                Path.GetRandomFileName() + ".jpeg");
            var args = $"-i {request.Location} -vframes 1 -an -s 200x200 -ss 2 {thumbnailPath}";
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "../ffmpeg",
                Arguments = args
            };
            Process proc = new Process()
            {
                StartInfo = startInfo,
            };
            proc.Start();
            this.Logger.LogInformation("Running:\n" + "ffmpeg " + args);

            var user = this.Context.User
                .Include(u => u.Videos)
                .Where(u => u.Id == Int32.Parse(userId.Value))
                .SingleOrDefault();

            var video = new Video()
            {
                Location = request.Location,
                ThumbnailLocation = thumbnailPath
            };

            user.Videos.Add(video);

            this.Context.Add(video);
            this.Context.Update(user);
            
            this.Context.SaveChanges();

            return Task.FromResult(this.Mapper.Map<Dtos.Video>(video));

        }
    }
}