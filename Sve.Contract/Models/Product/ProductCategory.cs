namespace Sve.Contract.Models.Product
{
        using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class ProductCategory
    {
        [JsonPropertyName("id")]
        public int CategoryId { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }
        public bool? HasSubCategory { get; set; }
        public int? ParentId { get; set; }
        public int? Status { get; set; }

        public ICollection<Brands> ProductBrands { get; set; }
        public ICollection<ProductDetails> ProductDetails { get; set; }
        public ICollection<Sizes> ProductSizes { get; set; }
        public ICollection<MaterialTypes> ProductMaterialType { get; set; }
        public ICollection<Colors> ProductColors { get; set; }
        public ICollection<Grades> ProductGrades { get; set; }
    }
}