namespace Sve.Contract.Interface.Logs
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Logs;

    public interface IErrorLogService
    {
		Task<(int? totalCount, List<ErrorLog> items)> GetByExpressionAsync(int index, int size, string sortColumn, string sortDirection, Filter<ErrorLog> filter = null);
        Task<ErrorLog> GetByIdAsync(int errorLogId);
        Task<ResponseResult> CreateAsync(ErrorLog entity);
        Task<ResponseResult> UpdateAsync(ErrorLog entity);
        Task<ResponseResult> DeleteByIdAsync(int[] errorLogIds);
    }
}