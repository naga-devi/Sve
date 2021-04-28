namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Accounts;

    public interface ITransactionDetailsService
    {
		Task<(int? totalCount, List<TransactionDetail> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, TransactionDetail filter = null, CancellationToken cancellationToken = default);
        Task<TransactionDetail> GetByIdAsync(int id, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(TransactionDetail entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(TransactionDetail entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default);
    }
}