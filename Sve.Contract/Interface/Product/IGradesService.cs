namespace Sve.Contract.Interface.Product
{
	using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Product;
    using JxNet.Core;
    public interface IGradesService
    {
		Task<(int? totalCount, List<Grades> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Grades filter = null, CancellationToken cancellationToken = default);
        Task<Grades> GetByIdAsync(int gradeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default);
        Task<ResponseResult> CreateAsync(Grades entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> UpdateAsync(Grades entity, CancellationToken cancellationToken = default);
        Task<ResponseResult> DeleteByIdAsync(int[] gradeIds, CancellationToken cancellationToken = default);
    }
}