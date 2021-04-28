namespace Sve.Service.Impl.Product
{
	using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Product;
    using Sve.Service.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using Models = Contract.Models.Product;
    using Domain = Service.Domain.Product;
    using JxNet.Extensions.CacheManager;
    using JxNet.Core;
    using JxNet.Core.Extensions;

    internal class GradesService : IGradesService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public GradesService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper, ICacheManager cacheManager)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

		public async Task<(int? totalCount, List<Models.Grades> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Grades filter = null, CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.Grades>()
                    .Select(x => new Models.Grades
                    {
                        GradeId = x.GradeId,
                        Name = x.Name,
                        CategoryId = x.CategoryId,
                        Category = new Models.ProductCategory
                        {
                            Name = x.Category.Name
                        }
                    })
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

                if((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.Grades>>( result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

		public async Task<Models.Grades> GetByIdAsync(int gradeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Grades>().Where(x => x.GradeId == gradeId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.Grades>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.Grades entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.Grades)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.Grades>(entity);

                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.Grades>().AnyAsync(x => x.CategoryId == entity.CategoryId && x.Name.ToLower().Equals(entity.Name.ToLower()), cancellationToken: cancellationToken))
                {
                    return new ResponseResult(2, Status.Error, string.Format(CommonConstants.ActionCommand_EntityAlreadyExistMessage, nameof(Models.Grades)));
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.GradeId;
                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.Grades)), ex);
            }

            return result;
        }

		public async Task<ResponseResult> UpdateAsync(Models.Grades entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.Grades)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.Grades>(entity.GradeId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.CategoryId = entity.CategoryId;
                    entityToUpdate.Name = entity.Name;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                scope.Complete();
                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.Grades)), ex);
            }

            return result;
        }        

        public async Task<ResponseResult> DeleteByIdAsync(int[] gradeIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.Grades)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                dbContext.RemoveByWhere<Domain.Grades>(x => gradeIds.Contains(x.GradeId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.Grades)), ex);
            }

            return result;
        }
    }
}
