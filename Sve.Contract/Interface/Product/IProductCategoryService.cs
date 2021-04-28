namespace Sve.Contract.Interface.Product
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JxNet.Core;
    using JxNet.Core.Linq;
    using Sve.Contract.Models.Product;

    public interface IProductCategoryService
    {
        Task<ICollection<ProductCategory>> GetCacheAllAsync();
        Task<ICollection<TaxSlabs>> GetCacheTaxSlabsAsync();

        //Task<ProductCategory> GetBransAndSizesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default(CancellationToken));
        Task<(int? totalCount, List<ProductCategory> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, ProductCategory filter = null, CancellationToken cancellationToken = default);
        Task<ProductCategory> GetByIdAsync(int categoryId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(ProductCategory entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(ProductCategory entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int?[] categoryIds, CancellationToken cancellationToken = default);
    }
}