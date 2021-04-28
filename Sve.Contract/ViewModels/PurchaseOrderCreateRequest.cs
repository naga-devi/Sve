namespace Sve.Contract.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class PurchaseOrderCreateRequest
    {
        public VendorCreateModel Vendor { get; set; }
        public List<PurchaseItems> Purchases { get; set; }
    }

    public class VendorCreateModel
    {
        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string TinNo { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public byte CgstInPercentage { get; set; }
        public decimal CgstAmount { get; set; }
        public byte SgstInPercentage { get; set; }
        public decimal SgstAmount { get; set; }
        public byte? IgstInPercentage { get; set; }
        public decimal? IgstAmount { get; set; }
        public decimal? Freight { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? RoundOffAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public int? PurchaseOrderId { get; set; }
    }

    public class PurchaseItems
    {

        /// <summary>
        /// Purchase details id
        /// </summary>
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public int? StockGroupId { get; set; }
        public int MaterialTypeId { get; set; }
        public int SizeId { get; set; }
        public int BrandId { get; set; }
        public int GradeId { get; set; }
        public int ColorId { get; set; }
        public decimal Mrp { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Discount { get; set; }

        public int UnitMeasureId { get; set; }
        public int? CategoryId { get; set; }
        public int Quanitity { get; set; }
        public decimal CgstAmount { get; set; }
        public decimal SgstAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool? IsPrime { get; set; }
        public short? MinimumStock { get; set; }
        public byte? RatingsCount { get; set; }
        public short? RatingsValue { get; set; }
        public string Description { get; set; }
    }
}
