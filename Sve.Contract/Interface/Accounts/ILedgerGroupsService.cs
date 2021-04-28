namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Accounts;

    public interface ILedgerGroupsService
    {
        Task<List<LedgerGroup>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<LedgerGroup> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, LedgerGroup filter = null, CancellationToken cancellationToken = default);
        Task<LedgerGroup> GetByIdAsync(int groupId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(LedgerGroup entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(LedgerGroup entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] groupIds, CancellationToken cancellationToken = default);
    }
}