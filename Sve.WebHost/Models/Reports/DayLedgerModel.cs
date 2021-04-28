namespace JxNet.Extensions.WebHost.Models.Reports
{
    public class DayLedgerViewModel
    {
        public string PaymodeText { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal? RoundOffAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}
