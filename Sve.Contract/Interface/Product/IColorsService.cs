namespace Sve.Contract.Interface.Product
{
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Product;
    using JxNet.Core;
    public interface IColorsService
    {
		Task<(int? totalCount, List<Colors> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Colors filter = null, CancellationToken cancellationToken = default);
        Task<Colors> GetByIdAsync(int colorId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Colors entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(Colors entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] colorIds, CancellationToken cancellationToken = default);
    }
}