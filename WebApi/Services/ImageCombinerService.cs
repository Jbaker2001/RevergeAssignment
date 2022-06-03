using WebApi.Models;

namespace WebApi.Services
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
        public async Task<bool> FitImagesInMaster(List<Image> images)
        {
            //Set up master image data
            Image masterImage = new Image(images[0].width, images[0].height, images[0].imageMatrix);
            masterImage = images[0];
            var masterImageArea = GetImageArea(masterImage);

            images.RemoveAt(0); // Remove master image from list of images

            List<int> imageAreas = new List<int>();

            for (int i = 0; i < images.Count(); i++)
                imageAreas.Add(GetImageArea(images[i]));

            //Make sure total image area is smaller than master image area
            var totalArea = await GetSumOfImageAreas(imageAreas);
            if (totalArea > masterImageArea)
                return false;

            List<Image> orderedList = await CreateOrderedList(images);
            AnchorPoint anchorPoint = new AnchorPoint();

            for (int i = 0; i < orderedList.Count(); i++)
            {
                Image currImage = orderedList[i];
                anchorPoint = FindNextAnchor(currImage, masterImage);

                if(anchorPoint.height != -1)
                {
                    masterImage.imageMatrix = CoverImage(anchorPoint, currImage.width, currImage.height, masterImage, i + 1);
                    Console.WriteLine("\n");
                }
                else
                {
                    return false;
                }
            }

            PrintMasterImage(masterImage);

            return true;
        }

        /**
         * FindNextAnchor
         * Purpose: Find next area that has 0 as value and return it
         * */
        public AnchorPoint FindNextAnchor(Image subImage, Image masterImage)
        {
            AnchorPoint anchor = new AnchorPoint();

            for (var i = 0; i < masterImage.height; i++)
            {
                for (var j = 0; j < masterImage.width; j++)
                {
                    if (masterImage.imageMatrix[i][j] == 0 && (masterImage.width - j) >= subImage.width && (masterImage.height - i) >= subImage.height)
                    {
                        anchor.width = j;
                        anchor.height = i;
                        return anchor;
                    }
                    else
                    {
                        var flippedImage = FlipImage(subImage);
                        if(masterImage.imageMatrix[i][j] == 0 && (masterImage.width - j) >= flippedImage.width && (masterImage.height - i) >= flippedImage.height)
                        {
                            anchor.width = j;
                            anchor.height = i;
                            return anchor;
                        }
                    }
                }
            }

            anchor.height = -1;

            return anchor;
        }

        /**
         * PrintMasterImage
         * Purpose: Print the master image in the console
         * */
        public void PrintMasterImage(Image img)
        {
            for (int i = 0; i < img.imageMatrix.Count(); i++)
            {
                var tempString = "";
                for (int j = 0; j < img.imageMatrix.Count(); j++)
                {
                    tempString += img.imageMatrix[i][j].ToString();
                }

                Console.WriteLine(tempString);
            }

            Console.WriteLine("\n\n");
        }

        /**
         * CoverImage
         * Purpose: Mark the areas of the master image as covered (1) for the size of 
         * the given image.
         * */
        public List<List<int>> CoverImage(AnchorPoint anchor, int width, int height, Image img, int value)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    img.imageMatrix[anchor.height + i][anchor.width + j] = value;

            return img.imageMatrix;
        }

        /**
         * FlipImage
         * Purpose: Flip the sides of the image and return it
         * */
        public Image FlipImage(Image img)
        {
            var temp = img.height;
            img.height = img.width;
            img.width = temp;

            return img;
        }

        /**
         * CreateOrderedList
         * Purpose: Create an ordered list largest-smallest from the 
         * original list of data.
         * */
        public async Task<List<Image>> CreateOrderedList(List<Image> list)
        {
            for (var i = 0; i < list.Count(); i++)
                for (var j = 0; j < list.Count(); j++)
                    if (list[i].width > list[j].width)
                        Swap(list, i, j);

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

            for (var i = 0; i < areas.Count(); i++)
                sum += areas[i];

            return sum;
        }
    }
}