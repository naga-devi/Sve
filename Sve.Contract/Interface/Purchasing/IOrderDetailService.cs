namespace Sve.Contract.Interface.Purchasing
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Product;
    using JxNet.Core;
    public interface IOrderDetailService
    {
        Task<List<StockGroups>> GetByOrderId(int purchaseOrderId, CancellationToken cancellationToken = default);
    }
}