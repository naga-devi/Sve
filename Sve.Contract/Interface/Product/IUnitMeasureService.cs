namespace Sve.Contract.Interface.Product
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JxNet.Core;
    using Sve.Contract.Models.Product;

    public interface IUnitMeasureService
    {
        Task<List<UnitMeasure>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<(int? totalCount, List<UnitMeasure> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, UnitMeasure filter = null, CancellationToken cancellationToken = default);
        Task<UnitMeasure> GetByIdAsync(int unitMeasureId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(UnitMeasure entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(UnitMeasure entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] unitMeasureIds, CancellationToken cancellationToken = default);
    }
}