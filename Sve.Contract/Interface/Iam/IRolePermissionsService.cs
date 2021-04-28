namespace Sve.Contract.Interface.Iam
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sve.Contract.Models.Iam;

    public interface IRolePermissionsService
    {
		Task<(int? totalCount, List<RolePermissions> items)> GetByExpressionAsync(int index, int size, string sortColumn, string sortDirection, Filter<RolePermissions> filter = null);
        Task<RolePermissions> GetByIdAsync(int rolePermissionId);
        Task<ResponseResult> CreateAsync(RolePermissions entity);
        Task<ResponseResult> UpdateAsync(RolePermissions entity);
        Task<ResponseResult> DeleteByIdAsync(int[] rolePermissionIds);
    }
}