using RevergeAssignment.Models;

namespace RevergeAssignment.Services
{
    public class ImageCombinerService
    {
        private readonly ILogger<ImageCombinerService> _logger;
        public ImageCombinerService(ILogger<ImageCombinerService> logger)
        {
            _logger = logger;
        }

        /**
         * FitImagesInMaster
         * Purpose: Main class for feature. Check if all other images fit in the 
         * master image.
         * */
        public async Task<bool> FitImagesInMaster(List<Image> images)
        {
            //Set up master image data
            Image masterImage = new Image(images[0].width, images[0].height);
            masterImage = images[0];
            var masterImageArea = GetImageArea(masterImage);

            images.RemoveAt(0); // Remove master image from list of images

            List<int> imageAreas = new List<int>();

            for(var i = 0; i < images.Count(); i++)
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

            var tempMasterImage = masterImage;
            var tempMaxWidth = masterImage.width;
            var tempMaxHeight = masterImage.height;

            for(var i = 0; i < orderedList.Count(); i++)
            {
                var currImage = orderedList[i];

                if(currImage.height > tempMaxHeight && currImage.width > tempMasterImage.width)
                    return false;

                if (currImage.width <= tempMaxWidth)
                {
                    tempMaxWidth -= currImage.width;
                    tempMaxHeight -= currImage.height;
                }
                else
                {
                    tempMasterImage.height = tempMaxHeight;
                }
            }

            return true;
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
