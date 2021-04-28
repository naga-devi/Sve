namespace Sve.Contract.Interface.Iam
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
	using Sve.Contract.Models.Iam;

    public interface IPermissionsService
    {
		Task<(int? totalCount, List<Permissions> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Permissions filter = null);
        Task<Permissions> GetByIdAsync(int permissionId);
        Task<ResponseResult> CreateAsync(Permissions entity);
        Task<ResponseResult> UpdateAsync(Permissions entity);
        Task<ResponseResult> DeleteByIdAsync(int[] permissionIds);
    }
}