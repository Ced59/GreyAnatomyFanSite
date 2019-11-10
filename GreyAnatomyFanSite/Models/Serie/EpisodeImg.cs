namespace GreyAnatomyFanSite.Models.Serie
{
    public class EpisodeImg
    {
        private string file_path;
        private int height;
        private int width;
        private double aspect_ratio;

        public string File_path { get => file_path; set => file_path = value; }
        public int Height { get => height; set => height = value; }
        public int Width { get => width; set => width = value; }
        public double Aspect_ratio { get => aspect_ratio; set => aspect_ratio = value; }
    }
}