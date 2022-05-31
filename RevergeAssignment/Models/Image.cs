namespace RevergeAssignment.Models
{
    public class Image
    {
        public int width { get; set; }
        public int height { get; set; }
        public Image(int _width, int _height)
        {
            width = _width;
            height = _height;
        }
    }
}
