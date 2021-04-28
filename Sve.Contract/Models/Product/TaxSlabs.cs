using System.Collections.Generic;

namespace Sve.Contract.Models.Product
{
    public class TaxSlabs
    {
        public TaxSlabs()
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