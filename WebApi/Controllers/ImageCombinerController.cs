using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    public class ImageCombinerController : ControllerBase
    {
        private readonly ILogger<ImageCombinerController> _logger;
        private readonly ImageCombinerService _iamgeCombinerService;
        private readonly FileReader _fileReaderService;

        public ImageCombinerController (ILogger<ImageCombinerController> logger,
            ImageCombinerService imageCombinerService,
            FileReader fileReader)
        {
            _logger = logger;
            _iamgeCombinerService = imageCombinerService;
            _fileReaderService = fileReader;
        }

        [HttpGet]
        [Route("/fitImages/fileString")]
        public async Task<IActionResult> FitImages(string fileString)
        {

            var results = await _iamgeCombinerService.FitImagesInMaster(fileString);

            if (results)
                return Ok();
            else
                return BadRequest("Images are not able to fit in the master image.");
        }
    }
}
