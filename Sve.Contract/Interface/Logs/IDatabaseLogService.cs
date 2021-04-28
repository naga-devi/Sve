namespace Sve.Contract.Interface.Logs
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Logs;

    public interface IDatabaseLogService
    {
		Task<(int? totalCount, List<DatabaseLog> items)> GetByExpressionAsync(int index, int size, string sortColumn, string sortDirection, Filter<DatabaseLog> filter = null);
        Task<DatabaseLog> GetByIdAsync(int databaseLogId);
        Task<ResponseResult> CreateAsync(DatabaseLog entity);
        Task<ResponseResult> UpdateAsync(DatabaseLog entity);
        Task<ResponseResult> DeleteByIdAsync(int[] databaseLogIds);
    }
}