namespace Sve.Contract.Models.Product
{
    using System.ComponentModel.DataAnnotations;

    public partial class ProductImages
    {
        public int? ProductId { get; set; }
        public int ImageId { get; set; }
        [Required]
        public string Path { get; set; }
        public int? ImageType { get; set; }
        public int? Status { get; set; }
    }
}
