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
        public async Task<List<Image>> ReadFile(string path)
        {
            List<Image> imageList = new List<Image>();

            foreach (var s in File.ReadLines($@"{path}"))
            {
                var text = s;
                string[] bits = text.Split(' ');
                int width = int.Parse(bits[0]);
                int height = int.Parse(bits[1]);
                List<List<int>> imgMatrix = await Create2DList(width, height);

                imageList.Add(new Image(width, height, imgMatrix));
            }

            return imageList;
        }

        /**
         * Create2DList
         * Purpose: Create a new 2D list for the iamge
         * */
        public async Task<List<List<int>>> Create2DList(int width, int height)
        {
            List<List<int>> list = new List<List<int>>();

            for(int i = 0; i < height - 2; i++)
            {
                List<int> subList = new List<int>();
                for(int s = 0; s < width - 2; s++)
                {
                    subList.Add(0);
                }

                list.Add(subList);
            }

            return list;
        }
    }
}
