using Microsoft.AspNetCore.Mvc;

namespace api.Controllers {

    [Route("media")]
    [ApiController]
    public class MediaController : Controller {

        [HttpPost("upload")]
        public IActionResult UploadVideo()
        {
            var http = Request;

            return this.Ok();
        }
        
    }
}