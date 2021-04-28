namespace Sve.Contract.Interface.Purchasing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Purchasing;
    using JxNet.Core;
    public interface IVendorsService
    {
        Task<(int? totalCount, List<Vendors> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Vendors filter = null, CancellationToken cancellationToken = default);
        Task<Vendors> GetByIdAsync(int vendorId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Vendors entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(Vendors entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] vendorIds, CancellationToken cancellationToken = default);

        Task<Vendors> GetByTinNoAsync(string tinNo);
        Task<Vendors> GetByPhoneNoAsync(string phoneNumber);
    }
}