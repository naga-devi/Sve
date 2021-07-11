namespace Sve.Contract.Interface.Purchasing
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Purchasing;

    public interface ICreditNotesService
    {
		Task<(int? totalCount, List<CreditNotes> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, CreditNotes filter = null, CancellationToken cancellationToken = default);
        Task<CreditNotes> GetByIdAsync(int id, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(CreditNotes entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(int purchaseOrderId, CreditNotes entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(CreditNotes entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default);
    }
}