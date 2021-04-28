using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JxNet.Core;
using Sve.Contract.Models.Product;

namespace Sve.Contract.Interface.Product
{
    public interface ISizesService
    {
		Task<(int? totalCount, List<Sizes> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Sizes filter = null, CancellationToken cancellationToken = default);
        Task<Sizes> GetByIdAsync(int sizeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Sizes entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(Sizes entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] sizeIds, CancellationToken cancellationToken = default);
    }
}