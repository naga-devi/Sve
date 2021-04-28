namespace Sve.Service.Domain.Product
{
    using System.Collections.Generic;

    internal partial class ProductTaxSlabs
    {
        public ProductTaxSlabs()
        {
            ProductDetails = new HashSet<ProductDetails>();
        }

        public int TaxSlabId { get; set; }
        public string Name { get; set; }
        public decimal TotalTax { get; set; }
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }

        public virtual ICollection<ProductDetails> ProductDetails { get; set; }
    }
}
