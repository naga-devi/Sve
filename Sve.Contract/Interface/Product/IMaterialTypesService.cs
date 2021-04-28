namespace Sve.Contract.Interface.Product
{
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JxNet.Core;
    using Sve.Contract.Models.Product;

    public interface IMaterialTypesService
    {
		Task<(int? totalCount, List<MaterialTypes> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, MaterialTypes filter = null, CancellationToken cancellationToken = default);
        Task<MaterialTypes> GetByIdAsync(int materialTypeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(MaterialTypes entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(MaterialTypes entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int?[] materialTypeIds, CancellationToken cancellationToken = default);
    }
}