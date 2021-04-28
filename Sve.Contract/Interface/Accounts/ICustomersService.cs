namespace Sve.Contract.Interface.Accounts
{
    using JxNet.Core;
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Accounts;

    public interface ICustomersService
    {
        Task<List<Customer>> GetAllAsync(int? groupId = default, string searchText= default, CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<Customer> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Customer filter = null, CancellationToken cancellationToken = default);
        Task<Customer> GetByIdAsync(int customerId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Customer entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(Customer entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] customerIds, CancellationToken cancellationToken = default);
    }
}