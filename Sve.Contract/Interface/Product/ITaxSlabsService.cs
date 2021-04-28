namespace Sve.Contract.Interface.Product
{
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JxNet.Core;
    using Sve.Contract.Models.Product;

    public interface ITaxSlabsService
    {
        Task<List<TaxSlabs>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<TaxSlabs> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, TaxSlabs filter = null, CancellationToken cancellationToken = default);
        Task<TaxSlabs> GetByIdAsync(int taxSlabId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(TaxSlabs entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(TaxSlabs entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] taxSlabIds, CancellationToken cancellationToken = default);
    }
}