using AGP.Snowden.ServiceLayer.Azure;
using AGPSnowden.Model;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.Files
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("UploadImageTmp")]
        public async Task<IActionResult> UploadImageTmp([FromForm] IFormFile file)
        {
            try
            {
                Response response = new Response();
                // Construct the URL to the blob
                if (file == null || file.Length == 0)
                    return BadRequest("File is not provided");

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    var imageUrl = await _imageService.UploadImageAsync(stream, file.FileName);
                    response.Data = imageUrl;
                    response.Success = true;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
