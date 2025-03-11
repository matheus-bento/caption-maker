using CaptionMaker.Files.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CaptionMaker.Files.Controllers;

[ApiController]
public class FileController : ControllerBase
{
    private readonly IOptions<CaptionMakerFilesOptions> _options;

    public FileController(IOptions<CaptionMakerFilesOptions> options)
    {
        this._options = options;
    }

    [HttpGet]
    [Route("file/{filename}")]
    public async Task<IActionResult> GetFile([FromRoute] string filename)
    {
        try
        {
            FileStream fs = await Task.Run(() =>
            {
                if (!System.IO.File.Exists($"{this._options.Value.BaseFilePath}/{filename}"))
                    throw new FileNotFoundException();

                return System.IO.File.OpenRead($"{this._options.Value.BaseFilePath}/{filename}");
            });

            return new FileStreamResult(fs, "image/jpeg");
        }
        catch (FileNotFoundException)
        {
            return NotFound(new ErrorResponse
            {
                Error = $"File '{filename}' was not found"
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

    [HttpPut]
    [Route("file")]
    public async Task<IActionResult> SaveFile()
    {
        try
        {
            if (this.Request.Body == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "No file found in the request body"
                });
            }

            string filename = Guid.NewGuid().ToString();
            string fileUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{filename}";

            using (FileStream fs = System.IO.File.Create($"{this._options.Value.BaseFilePath}/{filename}"))
            {
                await this.Request.Body.CopyToAsync(fs);
            }

            return Ok(new SaveFileSuccessResponse
            {
                Filename = filename,
                FileUrl = fileUrl
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
