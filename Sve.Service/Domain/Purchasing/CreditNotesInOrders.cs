namespace Sve.Service.Domain.Purchasing
{
    internal partial class CreditNotesInOrders
    {
        public int Id { get; set; }
        public int CreditNoteId { get; set; }
        public int PurchaseOrderId { get; set; }

        public virtual CreditNotes CreditNote { get; set; }
        public virtual PurchaseOrderHeader PurchaseOrder { get; set; }
    }
}
