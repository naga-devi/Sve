namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Accounts;

    public interface IPayModesService
    {
        Task<List<PayMode>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<PayMode> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, PayMode filter = null, CancellationToken cancellationToken = default);
        Task<PayMode> GetByIdAsync(int payModeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(PayMode entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(PayMode entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] payModeIds, CancellationToken cancellationToken = default);
    }
}