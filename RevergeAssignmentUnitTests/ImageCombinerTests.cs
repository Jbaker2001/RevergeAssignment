using Microsoft.Extensions.Logging;
using RevergeAssignment.Models;
using RevergeAssignment.Services;

namespace RevergeAssignmentUnitTests
{
    public class ImageCombinerTests
    {
        private readonly ILogger<ImageCombinerService> _iamgeCombinerLogger;
        private readonly ILogger<FileReader> _fileReaderLogger;

        [Fact]
        public async Task ImagesHaveSmallerPixelAreaThanMasterImage()
        {
            var service = new ImageCombinerService(_iamgeCombinerLogger);

            List<Image> imgList = new List<Image>();
            imgList.Add(new Image(80, 50));
            imgList.Add(new Image(20, 5));
            imgList.Add(new Image(7, 18));
            imgList.Add(new Image(7, 18));
            imgList.Add(new Image(6, 32));
            imgList.Add(new Image(48, 50));
            imgList.Add(new Image(3, 7));
            imgList.Add(new Image(80, 6));

            List<int> areaList = new List<int>();
            
            foreach(var i in imgList)
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