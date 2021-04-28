namespace Sve.Service.Domain.Sales
{
    internal partial class CustomersInOrders
    {
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public int CustomerId { get; set; }

        public Customers Customer { get; set; }
        public SalesOrderHeader SalesOrder { get; set; }
    }
}
