using CaptionMaker.Data.Model;
using CaptionMaker.Data.Repository;
using CaptionMaker.Model;
using CaptionMaker.Service.CaptionMaker;
using CaptionMaker.Service.ImageStorage;
using ImageMagick;
using ImageMagick.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaptionMaker.Controllers
{
    [Authorize]
    [ApiController]
    public class CaptionMakerController : ControllerBase
    {
        private readonly IImageStorageService _imageStorageService;
        private readonly ICaptionRepository _captionRepository;

        public CaptionMakerController(IImageStorageService imageStorageService, ICaptionRepository captionRepository)
        {
            this._imageStorageService = imageStorageService;
            this._captionRepository = captionRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("caption")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Caption> captions = await this._captionRepository.ListAsync();

                return Ok(
                    captions.Select(c => new CaptionListResponse
                    {
                        Filepath =
                            $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/caption/{c.Filepath}",
                        Username = c.User.Username
                    })
                );
            }
            catch
            {
                return StatusCode(500, new ErrorResponse
                {
                    Error = "An error occurred while processing the request"
                });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("caption/{filename}")]
        public async Task<IActionResult> Get([FromRoute] string filename)
        {
            try
            {
                if (String.IsNullOrEmpty(filename))
                {
                    return BadRequest("No filename informed");
                }

                Stream caption = await this._imageStorageService.GetAsync(filename);

                return new FileStreamResult(caption, "image/jpeg");
            }
            catch (FileNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Error = $"Caption '{filename}' was not found"
                });
            }
            catch
            {
                return StatusCode(500, new ErrorResponse
                {
                    Error = "An error occurred while processing the request"
                });
            }
        }

        [HttpPost]
        [Route("caption")]
        public async Task<IActionResult> Save([FromForm] SaveCaptionRequest req)
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

                    Queue<IDrawables<byte>> captions = await Task.Run(() => captionMaker.GenerateCaptions(req.Caption));

                    while (captions.Count > 0)
                    {
                        image.Draw(captions.Dequeue());
                    }

                    await image.WriteAsync(imageStream);
                }

                string filename = await this._imageStorageService.SaveAsync(imageStream);

                string username = this.User.Claims.First(c => c.Type == "name").Value;
                await this._captionRepository.SaveAsync(filename, username);

                return Ok(new SaveCaptionResponse
                {
                    CaptionUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/caption/{filename}"
                });
            }
            catch
            {
                return StatusCode(500, new ErrorResponse
                {
                    Error = "An error occurred while processing the request"
                });
            }
        }
    }
}
