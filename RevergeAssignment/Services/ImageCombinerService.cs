using RevergeAssignment.Models;

namespace RevergeAssignment.Services
{
    public class ImageCombinerService
    {
        private readonly ILogger<ImageCombinerService> _logger;
        private readonly FileReader _fileReaderService;
        public ImageCombinerService(ILogger<ImageCombinerService> logger,
            FileReader fileReaderService)
        {
            _logger = logger;
            _fileReaderService = fileReaderService;
        }

        /**
         * FitImagesInMaster
         * Purpose: Main class for feature. Check if all other images fit in the 
         * master image.
         * */
        public async Task<bool> FitImagesInMaster(string fileString)
        {
            //Read file to get images
            List<Image> images = await _fileReaderService.ReadFile(fileString);

            //Set up master image data
            Image masterImage = new Image(images[0].width, images[0].height, images[0].imageMatrix);
            masterImage = images[0];
            var masterImageArea = GetImageArea(masterImage);

            images.RemoveAt(0); // Remove master image from list of images

            List<int> imageAreas = new List<int>();

            for(int i = 0; i < images.Count(); i++)
                imageAreas.Add(GetImageArea(images[i]));

            //Make sure total image area is smaller than master image area
            var totalArea = await GetSumOfImageAreas(imageAreas);
            if(totalArea > masterImageArea)
                return false;

            List<Image> orderedList = new List<Image>();

            //Create ordered list based on largest side of master image
            if(masterImage.width > masterImage.height)
                orderedList = await CreateOrderedList(images, "width");
            else
                orderedList = await CreateOrderedList(images, "height");

            List<int> coords = new List<int>() { 0, 0};

            List<List<int>> matrix = masterImage.imageMatrix;

            for(int i = 0; i < orderedList.Count(); i++)
            {
                Image currImage = orderedList[i];
                bool imageFits = await CheckIfImageFits(coords, currImage.width, currImage.height, masterImage);

                if (imageFits)
                {
                    await CoverImage(coords, currImage.width, currImage.height, matrix);
                    coords[0] += currImage.width;
                }
                else
                {
                    var flippedImage = await FlipImage(currImage);
                    var flippedImageFits = await CheckIfImageFits(coords, flippedImage.width, flippedImage.height, masterImage);

                    if(!flippedImageFits)
                    {
                        await CoverImage(coords, currImage.width, currImage.height, matrix);
                        coords[0] += currImage.width;
                    }
                    else
                    {
                        List<int> anchor = FindNextAnchor(matrix);

                        if (anchor.Count() > 0)
                        {
                            coords[0] = anchor[0];
                            coords[1] = anchor[1];
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /**
         * FindNextAnchor
         * Purpose: Find next area that has 0 as value and return it
         * */
        public List<int> FindNextAnchor(List<List<int>> matrix)
        {
            List<int> coordList = new List<int>();

            for(int i = 0; i < matrix.Count(); i++)
            {
                for(int j = 0; j < matrix.Count(); j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        coordList.Add(j);
                        coordList.Add(i);
                        return coordList;
                    }
                }
            }

            return coordList;
        }

        /**
         * CoverImage
         * Purpose: Mark the areas of the master image as covered (1) for the size of 
         * the given image.
         * */
        public async Task<List<List<int>>> CoverImage(List<int> start, int width, int height, List<List<int>> matrix)
        {
            for(int i = start[1]; i < matrix.Count(); i++)
            {
                for(int j = start[0]; j < matrix.Count(); j++)
                {
                    matrix[i][j] = 1;
                }
            }

            return matrix;
        }

        /**
         * CheckIfImageFits
         * Purpose: Check necessary coordinates to see if an image will fit
         * */
        public async Task<bool> CheckIfImageFits(List<int> start, int width, int height, Image masterImage)
        {
            int startWidth = start[0];
            int startHeight = start[1];

            if ((masterImage.width - startWidth) > 0 && (masterImage.height - startHeight) > 0)
                return true;

            return false;
        }

        /**
         * FlipImage
         * Purpose: Flip the sides of the image and return it
         * */
        public async Task<Image> FlipImage(Image img)
        {
            var temp = img.height;
            img.height = img.width;
            img.width = temp;

            return img;
        }

        /**
         * GetSameHeightImages
         * Purpose: Get all images that are the same height of a given image and return them in 
         * a list.
         * */
        public async Task<List<Image>> GetSameHeightImages(int height, List<Image> list)
        {
            List<Image> newList = new List<Image>();

            foreach (var l in list) {
                if(l.height == height)
                {
                    newList.Add(l);
                }
            }

            return newList;
        }

        /**
         * CreateOrderedList
         * Purpose: Create an ordered list largest-smallest from the 
         * original list of data.
         * */
            public async Task<List<Image>> CreateOrderedList(List<Image> list, string orderBy) 
        {
            for(var i = 0; i < list.Count(); i++)
            {
                for(var j = 0; j < list.Count(); j++)
                {
                    switch(orderBy)
                    {
                        case "width":
                            if (list[i].width > list[j].width)
                                Swap(list, i, j);
                            break;
                        case "height":
                            if (list[i].height > list[j].height)
                                Swap(list, i, j);
                            break;
                        case "area":
                            if (GetImageArea(list[i]) > GetImageArea(list[j]))
                                Swap(list, i, j);
                            break;
                        default:
                            break;
                    }
                }
            }

            return list;
        }

        /**
         * Swap
         * Purpose: Swap values of list and return the updated list
         * */
        public List<Image> Swap(List<Image> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;

            return list;
        }

        /**
         * GetImageArea
         * Purpose: Get the area of an image then return the value
         * */
        public int GetImageArea(Image img)
        {
            return img.height * img.width;
        }

        /**
         * GetSumOfImageAreas
         * Purpose: Get the sum of all image areas and return the value
         * */
        public async Task<int> GetSumOfImageAreas(List<int> areas)
        {
            var sum = 0;

            for(var i = 0; i < areas.Count(); i++)
                sum += areas[i];

            return sum;
        }
    }
}
