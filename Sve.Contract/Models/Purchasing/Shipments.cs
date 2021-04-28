
namespace Sve.Contract.Models.Purchasing
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

    public class Shipments
    {
        public int ShipmentId { get; set; }
		public string MethodName { get; set; }
		public string VehicleNumber { get; set; }
		public string LRNumber { get; set; }
		public int? Status { get; set; }
    }
}