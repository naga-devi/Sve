namespace Sve.Contract.Models.Product
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class Grades
    {
		public int? CategoryId { get; set; }
        public int GradeId { get; set; }
		public string Name { get; set; }
        public ProductCategory Category { get; set; }
        public ICollection<StockGroups> StockGroups { get; set; }
    }
}