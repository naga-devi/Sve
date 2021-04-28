namespace Sve.Service.Impl.Product
{
    using AutoMapper;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.CacheManager;
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
    using Domain = Domain.Product;
    using Models = Contract.Models.Product;

    internal class ProductCategoryService : IProductCategoryService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        public ProductCategoryService(Func<ISveServiceDbContext> cdrDbContext, ICacheManager cacheManager, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task<ICollection<Models.ProductCategory>> GetCacheAllAsync()
        {
            var cacheResult = _cacheManager.Get<List<Models.ProductCategory>>(CacheKeys.ProductCategoryCacheKey.ToString());

            if (cacheResult != null)
            {
                return cacheResult;
            }

            using (var dbContext = _dbContext())
            {
                var list = await dbContext.GetAsQuerable<Domain.ProductCategory>()
                    .AsNoTracking()
                    .Select(x => new Models.ProductCategory
                    {
                        CategoryId = x.CategoryId,
                        HasSubCategory = x.HasSubCategory,
                        ParentId = x.ParentId,
                        Name = x.Name,
                        ProductSizes = x.ProductSizes.Select(s => new Models.Sizes
                        {
                            CategoryId = x.CategoryId,
                            Name = s.Name,
                            SizeId = s.SizeId
                        }).ToList(),
                        ProductBrands = x.ProductBrands.Select(b => new Models.Brands
                        {
                            CategoryId = x.CategoryId,
                            BrandId = b.BrandId,
                            Name = b.Name
                        }).ToList(),
                        ProductMaterialType = x.ProductMaterialType.Select(m => new Models.MaterialTypes
                        {
                            CategoryId = x.CategoryId,
                            MaterialTypeId = m.MaterialTypeId,
                            Name = m.Name
                        }).ToList(),
                        ProductColors = x.ProductColors.Select(m => new Models.Colors
                        {
                            CategoryId = x.CategoryId,
                            ColorId = m.ColorId,
                            Name = m.Name
                        }).ToList(),
                        ProductGrades = x.ProductGrades.Select(m => new Models.Grades
                        {
                            CategoryId = x.CategoryId,
                            GradeId = m.GradeId,
                            Name = m.Name
                        }).ToList()
                    })
                    .ToListAsync();

                if (list.HasItems())
                {
                    list.ForEach(x =>
                    {
                        x.ParentId ??= 0;
                    });

                    //var outlist = new List<Models.ProductCategory>();
                    //GetHirarchy(list, ref outlist, 0, "");

                    _cacheManager.Set(CacheKeys.ProductCategoryCacheKey.ToString(), list);

                    return list;
                }
            }

            return null;
        }

        public async Task<ICollection<Models.TaxSlabs>> GetCacheTaxSlabsAsync()
        {
            var cacheResult = _cacheManager.Get<List<Models.TaxSlabs>>(CacheKeys.TaxSlabs.ToString());

            if (cacheResult != null)
            {
                return cacheResult;
            }

            using (var dbContext = _dbContext())
            {
                var list = await dbContext.GetAsQuerable<Domain.ProductTaxSlabs>()
                    .AsNoTracking()
                    .Select(x => new Models.TaxSlabs
                    {
                        TaxSlabId = x.TaxSlabId,
                        Name = x.Name,
                        TotalTax = x.TotalTax,
                        Cgst = x.Cgst,
                        Sgst = x.Sgst
                    })
                    .ToListAsync();

                if (list.HasItems())
                {
                    _cacheManager.Set(CacheKeys.TaxSlabs.ToString(), list);

                    return list;
                }
            }

            return null;
        }

        //public async Task<Models.ProductCategory> GetBransAndSizesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    using (var dbContext = _dbContext())
        //    {
        //        var result = await dbContext.GetAsQuerable<Domain.ProductCategory>()
        //            .Where(x => x.CategoryId == categoryId)
        //            .Select(x => new Models.ProductCategory
        //            {
        //                ProductSizes = x.ProductSizes.Select(s => new Models.Sizes
        //                {
        //                    Name = s.Name,
        //                    SizeId = s.SizeId
        //                }).ToList(),
        //                ProductBrands = x.ProductBrands.Select(b => new Models.Brands
        //                {
        //                    BrandId = b.BrandId,
        //                    Name = b.Name
        //                }).ToList()
        //            })
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync();

        //        return _mapper.Map<Models.ProductCategory>(result);
        //    }
        //}

        public async Task<(int? totalCount, List<Models.ProductCategory> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.ProductCategory filter = null, CancellationToken cancellationToken = default)
        {

            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.ProductCategory>()
                .Select(x => new Models.ProductCategory
                {
                    CategoryId = x.CategoryId,
                    HasSubCategory = x.HasSubCategory,

                    ParentId = x.ParentId,
                    Name = x.Name,
                })
                .AsNoTracking()
                .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

            return (result?.TotalCount, result?.Items?.ToList());
        }

        public async Task<Models.ProductCategory> GetByIdAsync(int categoryId, bool? disbaleTracking = false, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.ProductCategory>().Where(x => x.CategoryId == categoryId);
            query = (bool)disbaleTracking ? query.AsNoTracking() : query;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Models.ProductCategory>(result);
        }

        public async Task<ResponseResult> CreateAsync(Models.ProductCategory entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.ProductCategory)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.ProductCategory>(entity);

                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.ProductCategory>().AnyAsync(x => x.Name.ToLower().Equals(entity.Name.ToLower()), cancellationToken: cancellationToken))
                {
                    return new ResponseResult(2, Status.Error, string.Format(CommonConstants.ActionCommand_EntityAlreadyExistMessage, nameof(Models.ProductCategory)));
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                result.NewId = entityToAdd.CategoryId;

                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.ProductCategory)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> UpdateAsync(Models.ProductCategory entity, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.ProductCategory)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.ProductCategory>(entity.CategoryId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.Name = entity.Name;
                    entityToUpdate.HasSubCategory = entity.HasSubCategory;
                    entityToUpdate.ParentId = entity.ParentId;
                    dbContext.Update(entityToUpdate);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());
                }

                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.ProductCategory)), ex);
            }

            return result;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int?[] categoryIds, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.ProductCategory)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.ProductDetails>().AnyAsync(x => categoryIds.Contains(x.CategoryId), cancellationToken: cancellationToken))
                {
                    return new ResponseResult(Status.Error, "Please delete the products before deleting the inventory group.");
                }

                dbContext.RemoveByWhere<Domain.ProductCategory>(x => categoryIds.Contains(x.CategoryId));
                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();
                _cacheManager.Remove(CacheKeys.ProductCategoryCacheKey.ToString());

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.ProductCategory)), ex);
            }

            return result;
        }

        #region Private method
        void GetHirarchy(List<Models.ProductCategory> objList, ref List<Models.ProductCategory> sortedList, int parentId, string prefix)
        {
            var list = from obj in objList where obj.ParentId == parentId select obj;

            if (parentId != 0)
                prefix += "...";

            foreach (Models.ProductCategory obj in list.ToList())
            {
                obj.Name = prefix + obj.Name;
                sortedList.Add(obj);

                GetHirarchy(objList, ref sortedList, obj.CategoryId, prefix);
            }
        }
        #endregion
    }
}
