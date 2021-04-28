namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Accounts;

    public interface IAccountDetailsService
    {
        Task<List<AccountDetail>> GetByCustomerId(int customerId, string searchText = default, CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<AccountDetail> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, AccountDetail filter = null, CancellationToken cancellationToken = default);
        Task<AccountDetail> GetByIdAsync(int accountId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(AccountDetail entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(AccountDetail entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] accountIds, CancellationToken cancellationToken = default);
    }
}