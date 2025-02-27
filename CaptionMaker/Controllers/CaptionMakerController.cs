using CaptionMaker.Model;
using CaptionMaker.Service;
using ImageMagick;
using ImageMagick.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaptionMaker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("caption")]
    public class CaptionMakerController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Caption([FromForm] CaptionRequest req)
        {
            try
            {
                if (req.Image == null)
                {
                    return BadRequest("No image included in the request");
                }

                if (String.IsNullOrEmpty(req.Caption))
                {
                    return BadRequest("No caption included in the request");
                }

                var imageStream = new MemoryStream();
                Stream reqImageStream = req.Image.OpenReadStream();

                using (var image = new MagickImage(reqImageStream))
                {
                    var captionMaker = new CaptionMakerService((int) image.Width);

                    // TODO: Run GenerateCaptions inside a Task for improved multithread performance
                    Queue<IDrawables<byte>> captions = captionMaker.GenerateCaptions(req.Caption);

                    while (captions.Count > 0)
                    {
                        image.Draw(captions.Dequeue());
                    }

                    await image.WriteAsync(imageStream);
                }

                imageStream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(imageStream, "image/jpeg");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
    }
}
