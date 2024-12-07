using System.Text.Json;
using ImageDetector.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageDetector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageProcessingController(ImageDetectionService imageDetectionService)
        : ControllerBase
    {
        private readonly ImageDetectionService _imageDetectionService = imageDetectionService;

        [HttpPost("detectobjects")]
        public async Task<IActionResult> ProcessImage([FromForm] IFormFile image)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);

                var detectionResult = await _imageDetectionService.DetectObjectsAsync(
                    memoryStream.ToArray()
                );

                return Ok(
                    new
                    {
                        message = "Image processed successfully",
                        data = JsonSerializer.Deserialize<object>(detectionResult)
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
