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

    internal class MaterialTypesService : IMaterialTypesService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public MaterialTypesService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper, ICacheManager cacheManager)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

		public async Task<(int? totalCount, List<Models.MaterialTypes> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.MaterialTypes filter = null, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.ProductMaterialTypes>()
                .Select(x => new Models.MaterialTypes
                {
                    MaterialTypeId = x.MaterialTypeId,
                    Name = x.Name,
                    CategoryId = x.CategoryId,
                    Category = new Models.ProductCategory
                    {
                        Name = x.Category.Name
                    }
                })
                .AsNoTracking()
                .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

            return (result?.TotalCount, result?.Items?.ToList() ?? null);
        }

		public async Task<Models.MaterialTypes> GetByIdAsync(int materialTypeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.ProductMaterialTypes>().Where(x => x.MaterialTypeId == materialTypeId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.MaterialTypes>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.MaterialTypes entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.MaterialTypes)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.ProductMaterialTypes>(entity);

                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.ProductMaterialTypes>().AnyAsync(x => x.CategoryId == entity.CategoryId && x.Name.ToLower().Equals(entity.Name.ToLower()), cancellationToken: cancellationToken))
                {
                    return new ResponseResult(2, Status.Error, string.Format(CommonConstants.ActionCommand_EntityAlreadyExistMessage, nameof(Models.MaterialTypes)));
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.MaterialTypeId;
                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.MaterialTypes)), ex);
            }

            return result;
        }

		public async Task<ResponseResult> UpdateAsync(Models.MaterialTypes entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.MaterialTypes)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.ProductMaterialTypes>(entity.MaterialTypeId);

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
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.MaterialTypes)), ex);
            }

            return result;
        }        

        public async Task<ResponseResult> DeleteByIdAsync(int?[] materialTypeIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.MaterialTypes)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.StockGroups>().AnyAsync(x => materialTypeIds.Contains(x.MaterialTypeId), cancellationToken: cancellationToken))
                {
                    return new ResponseResult(Status.Error, "Please delete the stock group before deleting the material type.");
                }

                dbContext.RemoveByWhere<Domain.ProductMaterialTypes>(x => materialTypeIds.Contains(x.MaterialTypeId));
                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.MaterialTypes)), ex);
            }

            return result;
        }
    }
}
