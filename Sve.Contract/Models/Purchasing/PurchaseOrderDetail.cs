
namespace Sve.Contract.Models.Purchasing
{
    using Sve.Contract.Models.Product;

    public class PurchaseOrderDetail
    {
        public int PurchaseOrderId { get; set; }
        public int UnitMeasureId { get; set; }
        public int StockGroupId { get; set; }
        public int Id { get; set; }
        public short ReceivedQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal RejectedQty { get; set; }
        public decimal StockedQty { get; set; }
        public decimal Mrp { get; set; }
        public decimal Discount { get; set; }
        public decimal CgstAmount { get; set; }
        public decimal SgstAmount { get; set; }
        public decimal IgstAmount { get; set; }
        public int Status { get; set; }
        public PurchaseOrderHeader PurchaseOrder { get; set; }
        public StockGroups StockGroup { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        
    }
}