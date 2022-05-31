using Microsoft.Extensions.Logging;
using WebApi.Services;
using WebApi.Controllers;
using WebApi.Models;

namespace WebApiUnitTests
{
    public class ImageCombinerTests
    {
        private readonly ILogger<ImageCombinerService> _imageCombinerLogger;
        private readonly ILogger<FileReader> _fileReaderLogger;

        private readonly FileReader _fileReader;

        [Fact]
        public async Task ImagesFitInMasterImage()
        {
            var service = new ImageCombinerService(_imageCombinerLogger, _fileReader);
            var fileReader = new FileReader(_fileReaderLogger);

            List<Image> list = await fileReader.ReadFile("C:\\projects\\RevergeAssignment\\WebApiUnitTests\\ExampleTextFile.txt");

            var results = await service.FitImagesInMaster(list);

            Assert.False(results);
        }


        [Fact]
        public async Task CreateOrderedList()
        {
            var service = new ImageCombinerService(_imageCombinerLogger, _fileReader);
            var fileReader = new FileReader(_fileReaderLogger);

            List<Image> list = await fileReader.ReadFile("C:\\projects\\RevergeAssignment\\WebApiUnitTests\\ExampleTextFile.txt");
            await service.CreateOrderedList(list);
        }

        [Fact]
        public async Task ListOfImagesIsReturned()
        {
            var service = new FileReader(_fileReaderLogger);

            var results = await service.ReadFile("C:\\projects\\RevergeAssignment\\WebApiUnitTests\\ExampleTextFile.txt");

            Assert.NotNull(results);
        }
    }
}