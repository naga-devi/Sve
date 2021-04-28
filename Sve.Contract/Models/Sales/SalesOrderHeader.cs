namespace Sve.Contract.Models.Sales
{
    using System;
    using System.Collections.Generic;

    public class SalesOrderHeader
    {
        public SalesOrderHeader()
        {
            OrderDetails = new HashSet<SalesOrderDetails>();
        }

        public int SalesOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount
        {
            get
            {

                if (DiscountPercentage > 0)
                {
                    return (DiscountPercentage / 100) * TotalAmount;
                }

                return 0;
            }
        }
        public decimal NetAmount { get; set; }
        public decimal? Freight { get; set; }
        public decimal? RoundOffAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public byte Paymode { get; set; }
        public string PaymodeText => Enum.GetName(typeof(Paymodes), Paymode);
        public string TransactionNo { get; set; }
        public string Comment { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public byte Status { get; set; }
        public string StatusText => Enum.GetName(typeof(SalesOrderStatus), Status);
        public ICollection<SalesOrderDetails> OrderDetails { get; set; }
        public ICollection<CustomersInOrders> CustomersInOrders { get; set; }
    }
}