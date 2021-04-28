namespace JxNet.Extensions.WebHost.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Images> Images { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? Discount { get; set; }
        public int? RatingsCount { get; set; }
        public int? RatingsValue { get; set; }
        public string Description { get; set; }
        public int? AvailibilityCount { get; set; }
        public int? CartCount { get; set; }
        public List<string> Color { get; set; }
        public List<string> Size { get; set; }
        public int? Weight { get; set; }
        public int? CategoryId { get; set; }
    }

    public class Images
    {
        [JsonPropertyName("id")]
        public int ImageId { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Big { get; set; }
    }
}
