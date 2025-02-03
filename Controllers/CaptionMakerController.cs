using ImageMagick.Drawing;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipelines;
using CaptionMaker.Model;

namespace CaptionMaker.Controllers
{
    [ApiController]
    [Route("caption")]
    public class CaptionMakerController : ControllerBase
    {
        private readonly ILogger<CaptionMakerController> _logger;

        public CaptionMakerController(ILogger<CaptionMakerController> logger)
        {
            _logger = logger;
        }

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
                    uint halfWidth = image.Width / 2;

                    new Drawables()
                        // Draw text on the image
                        .FontPointSize(72)
                        .Font("./static/Ubuntu.ttf")
                        .StrokeColor(MagickColors.Black)
                        .FillColor(MagickColors.White)
                        // Places the text in the middle of the screen
                        .Text(halfWidth, 80, req.Caption)
                        .TextAlignment(TextAlignment.Center)
                        .Draw(image);

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
