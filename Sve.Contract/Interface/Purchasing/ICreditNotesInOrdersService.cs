namespace Sve.Contract.Interface.Purchasing
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Purchasing;

    public interface ICreditNotesInOrdersService
    {
		Task<(int? totalCount, List<CreditNotesInOrders> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, CreditNotesInOrders filter = null, CancellationToken cancellationToken = default);
        Task<CreditNotesInOrders> GetByIdAsync(int id, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(CreditNotesInOrders entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(CreditNotesInOrders entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default);
    }
}