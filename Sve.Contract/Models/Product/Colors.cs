namespace Sve.Contract.Models.Product
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class Colors
    {
		public int? CategoryId { get; set; }
        public int ColorId { get; set; }
		public string Name { get; set; }
        public ProductCategory Category { get; set; }
        public ICollection<StockGroups> StockGroups { get; set; }
    }
}