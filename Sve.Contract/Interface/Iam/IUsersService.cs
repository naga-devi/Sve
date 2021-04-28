namespace Sve.Contract.Interface.Iam
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JxNet.Core;
    using Sve.Contract.Models.Iam;

    public interface IUsersService
    {
        Task<Users> AuthenticateAsync(string userName, string password);
        Task<(int? totalCount, List<Users> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Users filter = null);
        Task<Users> GetByIdAsync(int userId);
        Task<ResponseResult> CreateAsync(Users entity);
        Task<ResponseResult> UpdateAsync(Users entity);
        Task<ResponseResult> DeleteByIdAsync(int[] userIds);

        Task<ResponseDto<Users>> AddUserAsync(Users user, CancellationToken cancellationToken);
        Task<ResponseDto<Users>> LoginAsync(string userName, string password, CancellationToken cancellationToken = default);
    }
}