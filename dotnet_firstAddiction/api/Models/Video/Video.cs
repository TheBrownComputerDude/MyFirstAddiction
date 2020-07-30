namespace api.Models
{
    public class Video
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public string ContentType { get; set; }

        public string ThumbnailLocation { get; set; }

        public User User { get; set; }
    }
}