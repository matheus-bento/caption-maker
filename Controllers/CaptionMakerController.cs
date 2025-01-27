using ImageMagick.Drawing;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var imageStream = new MemoryStream();

            using (var image = new MagickImage(new MagickColor("#ff00ff"), 512, 128))
            {
                image.Format = MagickFormat.Jpeg;

                new Drawables()
                    // Draw text on the image
                    .FontPointSize(72)
                    .Font("./static/Ubuntu.ttf")
                    .StrokeColor(new MagickColor("yellow"))
                    .FillColor(MagickColors.Orange)
                    .TextAlignment(TextAlignment.Center)
                    .Text(256, 64, "Magick.NET")
                    // Add an ellipse
                    .StrokeColor(new MagickColor(0, Quantum.Max, 0))
                    .FillColor(MagickColors.SaddleBrown)
                    .Ellipse(256, 96, 192, 8, 0, 360)
                    .Draw(image);

                await image.WriteAsync(imageStream);

            }

            imageStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(imageStream, "image/jpeg");
        }
    }
}
