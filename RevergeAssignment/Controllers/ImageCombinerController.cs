using Microsoft.AspNetCore.Mvc;
using RevergeAssignment.Models;
using RevergeAssignment.Services;

namespace RevergeAssignment.Controllers
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
        [Route("/fitImages")]
        public async Task<IActionResult> FitImages()
        {
            List<Image> imageList = _fileReaderService.ReadFile("C:/projects/RevergeAssignment/RevergeAssignmentUnitTests/ExampleTextFile.txt");

            var results = await _iamgeCombinerService.FitImagesInMaster(imageList);

            if (results)
                return Ok();
            else
                return BadRequest("Images are not able to fit in the master image.");
        }
    }
}
