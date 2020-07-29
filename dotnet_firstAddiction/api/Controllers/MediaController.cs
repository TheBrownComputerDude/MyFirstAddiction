using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers {

    [Route("media")]
    [ApiController]
    public class MediaController : Controller {

        [HttpPost("upload")]
        [RequestSizeLimit(10000000)]
        [Authorize]
        public IActionResult UploadVideo(IFormCollection collection)
        {
            var file = collection.Files.FirstOrDefault();
            if (file == null)
            {
                return this.BadRequest();
            }

            var path = "../videoUploads";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filePath = Path.Combine(path, file.FileName);

            using (var fileStream = System.IO.File.Open(filePath, FileMode.OpenOrCreate))
            {
                file.CopyTo(fileStream);
            }

            return this.Ok();
        }
        
    }
}