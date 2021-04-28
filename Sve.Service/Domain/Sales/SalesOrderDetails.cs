namespace Sve.Service.Domain.Sales
{
    using Sve.Service.Domain.Product;

    internal partial class SalesOrderDetails
    {
        public int SalesOrderId { get; set; }
        public int Id { get; set; }
        public int? StockGroupId { get; set; }
        public short OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CgstAmount { get; set; }
        public decimal SgstAmount { get; set; }
        public decimal? IgstAmount { get; set; }
        public int? Status { get; set; }
        public int? UnitMeasureId { get; set; }

        public SalesOrderHeader SalesOrder { get; set; }
        public StockGroups StockGroup { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
    }
}
