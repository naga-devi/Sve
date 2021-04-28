namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Accounts;

    public interface IVoucherTypesService
    {
        Task<List<VoucherType>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<VoucherType> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, VoucherType filter = null, CancellationToken cancellationToken = default);
        Task<VoucherType> GetByIdAsync(int voucherTypeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(VoucherType entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(VoucherType entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] voucherTypeIds, CancellationToken cancellationToken = default);
    }
}