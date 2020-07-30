using System.IO;
using System.Linq;
using api.Commands;
using api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers {

    [Route("media")]
    [ApiController]
    public class MediaController : Controller {
        public MediaController(IMediator mediator, FirstAddictionContext context)
        {
            this.Mediator = mediator;
            this.Context = context;
        }

        private IMediator Mediator { get; }

        private FirstAddictionContext Context { get; }

        [HttpPost("upload")]
        [RequestSizeLimit(10000000)]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> UploadVideoAsync(IFormFile video)
        {
            if (video == null)
            {
                return this.BadRequest();
            }
            // TODO: need to add output location in config
            var path = "../videoUploads";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filePath = Path.Combine(path, video.FileName);

            using (var fileStream = System.IO.File.Open(filePath, FileMode.OpenOrCreate))
            {
                video.CopyTo(fileStream);
            }
            await this.Mediator.Send(new AddVideoCommand()
            {
                Location = filePath,
                ContentType = video.ContentType
            });

            return this.Ok();
        }

        [HttpGet("thumbnail")]
        [Authorize]
        public IActionResult GetVideoThumbnail(int videoId)
        {
            var video = this.Context.Video
                .Where(v => v.Id == videoId)
                .SingleOrDefault();

            if (video is null)
            {
                return this.BadRequest();
            }
            var fs = System.IO.File.OpenRead(video.ThumbnailLocation);
            var data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            return File(data, "image/jpeg");
        }

        [HttpGet("video")]
        [Authorize]
        public IActionResult GetVideo(int videoId)
        {
            var video = this.Context.Video
                .Where(v => v.Id == videoId)
                .SingleOrDefault();

            if (video is null)
            {
                return this.BadRequest();
            }
            var fs = System.IO.File.OpenRead(video.Location);
            var data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            return File(data, video.ContentType);
        }
        
    }
}