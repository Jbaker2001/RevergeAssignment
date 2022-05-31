using RevergeAssignment.Models;

namespace RevergeAssignment.Services
{
    public class FileReader
    {
        private readonly ILogger<FileReader> _logger;

        public FileReader(ILogger<FileReader> logger)
        {
            _logger = logger;
        }

        /**
         * ReadFile
         * Purpose: Read a plain text file line by line and return a list of 
         * image objects
         * */
        public List<Image> ReadFile(string path)
        {
            List<Image> imageList = new List<Image>();

            foreach (var s in System.IO.File.ReadLines($@"{path}"))
            {
                var text = s;
                string[] bits = text.Split(' ');
                int width = int.Parse(bits[0]);
                int height = int.Parse(bits[1]);

                imageList.Add(new Image(width, height));
            }

            return imageList;
        }
    }
}
