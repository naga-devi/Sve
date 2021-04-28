
namespace Sve.Contract.Models.Sales
{
    public class Customers
    {
        public int CustomerId { get; set; }
		public string CompanyName { get; set; }
		public string Name { get; set; }
        public string Email { get; set; }
		public string PhoneNo { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
        public int? StateID { get; set; } = 1;
		public string PAN { get; set; }
		public string TinNo { get; set; }
		public string CSTNo { get; set; }
        public int? Status { get; set; } = 1;
    }

    public partial class CustomersInOrders
    {
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public int CustomerId { get; set; }

        public Customers Customer { get; set; }
        public SalesOrderHeader SalesOrder { get; set; }
    }
}