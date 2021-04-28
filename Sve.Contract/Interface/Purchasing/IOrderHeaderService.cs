namespace Sve.Contract.Interface.Purchasing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Purchasing;
    using Sve.Contract.ViewModels;
    using JxNet.Core;
    public interface IOrderHeaderService
    {
        Task<(int? totalCount, List<PurchaseOrderHeader> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, PurchaseOrderHeader filter = null, CancellationToken cancellationToken = default);
        //Task<PurchaseOrderHeader> GetByIdAsync(int purchaseOrderId, bool? disbaleTracking = false, CancellationToken cancellationToken = default(CancellationToken));
        Task<ResponseResult> SaveAsync(PurchaseOrderCreateRequest request, CancellationToken cancellationToken = default);
        Task<PurchaseOrderHeader> GetById(int purchaseOrderId);
        //Task<ResponseResult> UpdateAsync(PurchaseOrderHeader entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default);
    }
}