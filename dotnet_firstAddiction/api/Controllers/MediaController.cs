using System.IO;
using System.Linq;
using api.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers {

    [Route("media")]
    [ApiController]
    public class MediaController : Controller {
        public MediaController(IMediator mediator)
        {
            this.Mediator = mediator;   
        }

        private IMediator Mediator { get; }

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
                Location = filePath
            });

            return this.Ok();
        }
        
    }
}