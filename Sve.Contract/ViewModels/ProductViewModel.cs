namespace Sve.Contract.ViewModels
{
    using Sve.Contract.Models.Product;
    using System.Collections.Generic;

    public class ProductViewModel : ProductListModel
    {
        //public List<int?> Sizes { get; set; }
        //public List<int?> Brands { get; set; }

        public List<Sizes> Sizes { get; set; }
        public List<Brands> Brands { get; set; }
        public List<MaterialTypes> MaterialTypes { get; set; }
        public List<Colors> Colors { get; set; }
        public List<Grades> Grades { get; set; }
    }
}
