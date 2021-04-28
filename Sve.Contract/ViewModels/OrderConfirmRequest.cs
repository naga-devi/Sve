
namespace Sve.Contract.ViewModels
{
    using Sve.Contract.Models.Sales;

    public class OrderConfirmRequest
    {
        public bool IsOrderChanged { get; set; }
        public SalesOrderHeader SalesOrder { get; set; }
        public Customers Customer { get; set; }

    }
}
