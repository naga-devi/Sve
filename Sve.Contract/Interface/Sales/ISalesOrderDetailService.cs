namespace Sve.Contract.Interface.Sales
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Sales;
    using JxNet.Core;
    public interface IOrderDetailService
    {
		Task<(int? totalCount, List<SalesOrderDetails> items)> GetByExpressionAsync(int salesOrderId, int index, int size, string sortColumn, bool isDescending, SalesOrderDetails filter = null);
        Task<List<Models.Product.StockGroups>> GetByOrderId(int salesOrderId, CancellationToken cancellationToken = default);
    }
}