namespace Sve.Contract.Interface.Purchasing
{
    using JxNet.Core;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Purchasing;
    using Sve.Contract.ViewModels;

    public interface IPurchaseReturnsService
    {
		Task<(int? totalCount, List<PurchaseReturns> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, PurchaseReturns filter = null, CancellationToken cancellationToken = default);
        Task<PurchaseReturns> GetByIdAsync(int id, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(PurchaseReturnCreateModel request, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(PurchaseReturns entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default);
    }
}