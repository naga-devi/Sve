namespace Sve.Contract.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class InvoiceModel
    {
        public int InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string TransportName { get; set; }
        public string VehicleNo { get; set; }
        public string PlaceOfSupply { get; set; }
        public string RupeesInWords { get; set; }
        public decimal? TotalAmountBeforeTax { get; set; }
        public decimal? TotalAmountAfterTax { get; set; }
        public decimal? Discount { get; set; }
        public string AddCGST { get; set; }
        public string AddSGST { get; set; }
        public string AddTransport { get; set; }
        public CustomerInfo Customer { get; set; }
        public List<InvoiceItems> InvoiceItems { get; set; }
    }

    public class CustomerInfo
    {
        public string Address { get; set; }
        public string CosumerName { get; set; }
        public string CellNo { get; set; }
        public string GSTIN { get; set; }
    }

    public class InvoiceItems
    {
        public int Id { get; set; }
        public int? StockGroupId { get; set; }
        public short OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Math.Round(OrderQty * UnitPrice, 2);
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }
        public decimal? Igst { get; set; }

        public decimal? SgstInPercentage { get; set; }
        public decimal? CgstInPercentage { get; set; }
        public decimal? IgstInPercentage { get; set; }

        public decimal? TaxAmount => Cgst + Sgst + Igst;
    }
}
