namespace Sve.Contract.Interface.Iam
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Iam;

    public interface IRoleService
    {
		Task<(int? totalCount, List<Role> items)> GetByExpressionAsync(int index, int size, string sortColumn, string sortDirection, Filter<Role> filter = null);
        Task<Role> GetByIdAsync(int roleId);
        Task<ResponseResult> CreateAsync(Role entity);
        Task<ResponseResult> UpdateAsync(Role entity);
        Task<ResponseResult> DeleteByIdAsync(int[] roleIds);
    }
}