using JxNet.Extensions.EFCore.SqlServer;
using System.Collections.Generic;

namespace Sve.Service.Domain.Product
{
    internal partial class ProductCategory : AuditEntityBase
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool? HasSubCategory { get; set; }
        public int? ParentId { get; set; }
        public int? Status { get; set; }

        public ICollection<ProductDetails> ProductDetails { get; set; }
        public ICollection<ProductBrands> ProductBrands { get; set; }
        public ICollection<ProductSizes> ProductSizes { get; set; }
        public ICollection<ProductMaterialTypes> ProductMaterialType { get; set; }
        public ICollection<Colors> ProductColors { get; set; }
        public ICollection<Grades> ProductGrades { get; set; }
    }
}