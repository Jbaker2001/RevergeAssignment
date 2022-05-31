namespace WebApi.Models
{
    public class Image
    {
        public int width { get; set; }
        public int height { get; set; }
        public List<List<int>> imageMatrix { get; set; }
        public Image(int _width, int _height, List<List<int>>_imgMatrix)
        {
            width = _width;
            height = _height;
            imageMatrix = _imgMatrix;
        }
    }
}
