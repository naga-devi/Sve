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
    internal class SizesService : ISizesService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public SizesService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper, ICacheManager cacheManager)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

		public async Task<(int? totalCount, List<Models.Sizes> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.Sizes filter = null, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.ProductSizes>()
                .Select(x => new Models.Sizes
                {
                    SizeId = x.SizeId,
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

		public async Task<Models.Sizes> GetByIdAsync(int sizeId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.ProductSizes>().Where(x => x.SizeId == sizeId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.Sizes>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.Sizes entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.Sizes)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.ProductSizes>(entity);

                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.ProductSizes>().AnyAsync(x => x.CategoryId == entity.CategoryId && x.Name.ToLower().Equals(entity.Name.ToLower()), cancellationToken: cancellationToken))
                {
                    return new ResponseResult(2, Status.Error, string.Format(CommonConstants.ActionCommand_EntityAlreadyExistMessage, nameof(Models.Sizes)));
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.SizeId;
                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.Sizes)), ex);
            }

            return result;
        }

		public async Task<ResponseResult> UpdateAsync(Models.Sizes entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.Sizes)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.ProductSizes>(entity.SizeId);

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
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.Sizes)), ex);
            }

            return result;
        }        

        public async Task<ResponseResult> DeleteByIdAsync(int[] sizeIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.Sizes)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.StockGroups>().AnyAsync(x => sizeIds.Contains(x.SizeId), cancellationToken: cancellationToken))
                {
                    return new ResponseResult(Status.Error, "Please delete the stock group before deleting the size.");
                }

                dbContext.RemoveByWhere<Domain.ProductSizes>(x => sizeIds.Contains(x.SizeId));
                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.Sizes)), ex);
            }

            return result;
        }
    }
}
