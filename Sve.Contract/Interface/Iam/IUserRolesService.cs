namespace Sve.Contract.Interface.Iam
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Iam;

    public interface IUserRolesService
    {
		Task<(int? totalCount, List<UserRoles> items)> GetByExpressionAsync(int index, int size, string sortColumn, string sortDirection, Filter<UserRoles> filter = null);
        Task<UserRoles> GetByIdAsync(int userRolesId);
        Task<ResponseResult> CreateAsync(UserRoles entity);
        Task<ResponseResult> UpdateAsync(UserRoles entity);
        Task<ResponseResult> DeleteByIdAsync(int[] userRolesIds);
    }
}