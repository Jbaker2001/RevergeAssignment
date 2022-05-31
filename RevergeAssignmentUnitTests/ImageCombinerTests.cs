using Microsoft.Extensions.Logging;
using RevergeAssignment.Controllers;
using RevergeAssignment.Models;
using RevergeAssignment.Services;

namespace RevergeAssignmentUnitTests
{
    public class ImageCombinerTests
    {
        private readonly ILogger<ImageCombinerService> _iamgeCombinerLogger;
        private readonly ILogger<FileReader> _fileReaderLogger;

        private readonly ILogger<ImageCombinerController> _imageCombinerLogger;

        [Fact]
        public async Task ImagesHaveSmallerPixelAreaThanMasterImage()
        {
            var service = new ImageCombinerService(_iamgeCombinerLogger);

            foreach(var i in imgList)
            {
                areaList.Add(service.GetImageArea(i));
            }

            var results = await service.FitImagesInMaster(imgList);

            Assert.False(results);
        }

        [Fact]
        public async Task ImagesHaveLargerAreaThanMasterImage()
        {
            var service = new ImageCombinerService(_iamgeCombinerLogger);

            List<Image> imgList = new List<Image>();

            foreach (var i in imgList)
            {
                areaList.Add(service.GetImageArea(i));
            }

            var results = await service.FitImagesInMaster(imgList);

            Assert.False(results);
        }

        [Fact]
        public async Task ListOfImagesIsReturned()
        {
            var service = new FileReader(_fileReaderLogger);

            var results = service.ReadFile("C:/projects/RevergeAssignment/RevergeAssignmentUnitTests/ExampleTextFile.txt");
        }
    }
}