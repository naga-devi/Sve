
namespace Sve.Contract.Models.Product
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class UnitMeasure
    {
        public int UnitMeasureId { get; set; }
		[Required]
		public string UnitMeasureCode { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
		public string Name { get; set; }
		public DateTime ModifiedDate { get; set; }
    }
}