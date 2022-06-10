namespace SimpleApi.Models
{
    public class AdvertisementModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }

        public IEnumerable<IFormFile> Files { get; set; }
    }
}
