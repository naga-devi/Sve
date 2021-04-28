namespace JxNet.Extensions.WebHost.Models
{
    public class CheckOutModel
    {
        //public string Name { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int TaxSlabId { get; set; }
        public int ProductBaseId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public short Quantity { get; set; }
        internal decimal LineTotal => UnitPrice + Cgst + Sgst;
        internal decimal TotalAmount => LineTotal * Quantity;
    }
}
