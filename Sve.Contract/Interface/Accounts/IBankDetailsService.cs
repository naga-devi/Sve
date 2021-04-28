namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Accounts;

    public interface IBankDetailsService
    {
        Task<List<BankDetail>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<BankDetail> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, BankDetail filter = null, CancellationToken cancellationToken = default);
        Task<BankDetail> GetByIdAsync(int bankId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(BankDetail entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(BankDetail entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] bankIds, CancellationToken cancellationToken = default);
    }
}