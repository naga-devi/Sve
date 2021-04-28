namespace Sve.Contract.Interface.Product
{
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JxNet.Core;
    using Sve.Contract.Models.Product;

    public interface IBrandsService
    {
		Task<(int? totalCount, List<Brands> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Brands filter = null, CancellationToken cancellationToken = default);
        Task<Brands> GetByIdAsync(int brandId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Brands entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(Brands entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] brandIds, CancellationToken cancellationToken = default);
    }
}