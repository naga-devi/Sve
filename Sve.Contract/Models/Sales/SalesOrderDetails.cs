namespace Sve.Contract.Models.Sales
{
    using Sve.Contract.Models.Product;

    public class SalesOrderDetails
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

        public decimal? LineTotal
        {
            get
            {
                IgstAmount = IgstAmount ?? 0;
                return (UnitPrice + CgstAmount + SgstAmount + IgstAmount) * OrderQty;
            }
        }

        public SalesOrderHeader SalesOrder { get; set; }
        public StockGroups StockGroup { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
    }
}