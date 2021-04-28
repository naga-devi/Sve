namespace JxNet.Extensions.WebHost.Models
{
    public class ProductStockCreateRequest
    {
        public int? ProductId { get; set; }
        public int StockGroupId { get; set; }
        public int? MaterialTypeId { get; set; }
        public int SizeId { get; set; }
        public int BrandId { get; set; }
        public decimal NetPrice { get; set; }
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }
        public decimal? Igst { get; set; }
        public decimal? Discount { get; set; }
        public decimal? BuyPrice { get; set; }
        public int StockQty { get; set; }
    }

    public class ProductStockUpdateRequest
    {
        public int StockGroupId { get; set; }
        public decimal NetPrice { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SellPrice { get; set; }
    }

    public class PurchaseCreateRequest
    {
        public int StockGroupId { get; set; }
        public decimal BuyPrice { get; set; }
        public int StockedQty { get; set; }
    }
}
