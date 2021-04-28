namespace Sve.Service.Impl.Iam
{
    using JxNet.Extensions.CacheManager;
    using Sve.Contract.Interface.Iam;
    using Models = Sve.Contract.Models.Iam;
    using Sve.Service.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using JxNet.Core;
    internal class UserService : IUsersService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly ICacheManager _cacheManager;

        public UserService(Func<ISveServiceDbContext> cdrDbContext, ICacheManager cacheManager)
        {
            _dbContext = cdrDbContext;
            _cacheManager = cacheManager;
        }

        public async Task<Models.Users> AuthenticateAsync(string userName, string password)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Iam.Users>()
                .Where(x => x.UserName.ToLower().Equals(userName.ToLower()) && x.Password.Equals(password))
                .AsNoTracking()
                .Select(x => new Models.Users
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Contactno = x.Contactno,
                    EmailId = x.EmailId,
                    UserTypeId = x.UserTypeId
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public Task<ResponseResult> CreateAsync(Models.Users entity)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult> DeleteByIdAsync(int[] userIds)
        {
            throw new NotImplementedException();
        }

        public Task<(int? totalCount, List<Models.Users> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Users filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Users> GetByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
        
        public Task<ResponseResult> UpdateAsync(Models.Users entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<Models.Users>> LoginAsync(string userName, string password, CancellationToken cancellationToken)
        {
            using var dbContext = _dbContext();
            var userModel = await dbContext.GetAsQuerable<Domain.Iam.Users>()
                .Where(x => x.UserName.ToLower().Equals(userName.ToLower()) && x.Password.Equals(password))
                .AsNoTracking()
                .Select(x => new Models.Users
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Contactno = x.Contactno,
                    EmailId = x.EmailId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (userModel == null)
            {
                return ResponseDto<Models.Users>.UnsuccessfulResponse
                    (HttpResponseEnum.Forbidden, "User not found.");
            }

            return ResponseDto<Models.Users>.SuccessfulResponse(userModel);
        }

        public Task<ResponseDto<Models.Users>> AddUserAsync(Models.Users user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
