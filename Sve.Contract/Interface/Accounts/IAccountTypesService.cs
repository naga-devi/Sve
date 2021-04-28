namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Accounts;

    public interface IAccountTypesService
    {
        Task<List<AccountType>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<AccountType> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, AccountType filter = null, CancellationToken cancellationToken = default);
        Task<AccountType> GetByIdAsync(int accountTypeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(AccountType entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(AccountType entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] accountTypeIds, CancellationToken cancellationToken = default);
    }
}