namespace Sve.Contract.Interface.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Sales;
    using JxNet.Core;
    public interface ICustomersService
    {
        Task<Customers> GetByPhoneNoAsync(string phoneNumber);
        Task<Customers> GetByTinNoAsync(string tinNo);
        Task<(int? totalCount, List<Customers> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Customers filter = null, CancellationToken cancellationToken = default);
        Task<Customers> GetByIdAsync(int customerID, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Customers entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(Customers entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] customerIds, CancellationToken cancellationToken = default);
    }
}