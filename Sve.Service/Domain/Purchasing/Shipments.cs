using System;
using System.Collections.Generic;

namespace Sve.Service.Domain.Purchasing
{
    public partial class Shipments
    {
        public int ShipmentId { get; set; }
        public string MethodName { get; set; }
        public string VehicleNumber { get; set; }
        public string Lrnumber { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? Status { get; set; }
    }
}
