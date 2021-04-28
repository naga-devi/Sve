namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Accounts;

    public interface ITransactionsService
    {
		Task<(int? totalCount, List<Transactions> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Transactions filter = null, CancellationToken cancellationToken = default);
        Task<Transactions> GetByIdAsync(int transactionId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Transactions entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Transactions entity, TransactionDetail transactionDetail, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(Transactions entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(long[] transactionIds, CancellationToken cancellationToken = default);
    }
}