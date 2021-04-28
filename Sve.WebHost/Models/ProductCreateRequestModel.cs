namespace JxNet.Extensions.WebHost.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class ProductCreateRequestModel
    {
        public int? TaxSlabId { get; set; }
        public int? CategoryId { get; set; }
        [JsonPropertyName("id")]
        public int ProductId { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string Name { get; set; }
		public int Hsn { get; set; }
		public short? MinimumStock { get; set; }
		public byte? RatingsCount { get; set; }
		public short? RatingsValue { get; set; }
        public string Description { get; set; }
        public List<ProductImageModel> AddedImages { get; set; }
        public List<int> DeletedImages { get; set; }
    }

    public class ProductImageModel
    {
        [JsonPropertyName("data")]
        public string Base64Data { get; set; }
        public string Name { get; set; }
    }
}
