using System;
using System.Collections.Generic;
using System.Text;

namespace Sve.Contract.Models.Product
{
    public class MaterialTypes
    {
        public int? CategoryId { get; set; }
        public int? MaterialTypeId { get; set; }
        public string Name { get; set; }
        public ProductCategory Category { get; set; }
    }
}
