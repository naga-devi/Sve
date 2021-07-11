namespace Sve.Service.Impl.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading.Tasks;
    using System.Transactions;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract;
    using Sve.Contract.Interface.Product;
    using Sve.Contract.ViewModels;
    using Sve.Service.Data;
    using Models = Contract.Models.Product;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class ProductDetailsService : IProductDetailsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public ProductDetailsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<List<Models.ProductDetails>> GetAllProductsAsync()
        {
            using var dbContext = _dbContext();
            return await dbContext.GetAsQuerable<Domain.Product.ProductDetails>()
                .Select(x => new Models.ProductDetails
                {
                    Name = x.Name,
                    ProductId = x.ProductId,
                    CategoryId = x.CategoryId,
                    TaxSlabId = x.TaxSlabId
                }).ToListAsync();
        }

        public async Task<(int? totalCount, List<ProductListModel> items)> GetProductsAsync(ProductFilterViewModel filter)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Product.ProductDetails>();
            query = filter.CategoryId > 0 ? query.Where(x => x.CategoryId == filter.CategoryId) : query;
            query = !filter.Name.IsNullOrEmpty() ? query.Where(x => x.Name.Contains(filter.Name)) : query;
            query = filter.StartPrice > 0 ? query.Where(x => x.StockGroups.Any(p => p.SellPrice > filter.StartPrice)) : query;
            query = filter.EndPrice > 0 ? query.Where(x => x.StockGroups.Any(p => p.SellPrice < filter.EndPrice)) : query;
            query = filter.SizeIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.SizeIds.Contains(p.SizeId))) : query;
            query = filter.BrandIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.BrandIds.Contains(p.BrandId))) : query;
            query = filter.MaterialTypeIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.MaterialTypeIds.Contains(p.MaterialTypeId))) : query;
            query = filter.ColorIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.ColorIds.Equals(p.ColorId))) : query;
            query = filter.GradeIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.GradeIds.Contains(p.GradeId))) : query;

            switch (filter.SortBy)
            {
                case 1: //default
                    query = query.OrderBy(x => x.Name);
                    break;
                case 2: // best match
                    query = query.OrderBy(x => x.StockGroups.OrderBy(o => o.Discount));
                    break;
                case 3: // lowest price
                    query = query.OrderBy(x => x.StockGroups.OrderBy(o => o.SellPrice));
                    break;
                case 4: // highest price
                    query = query.OrderByDescending(x => x.StockGroups.OrderByDescending(o => o.SellPrice));
                    break;
            }

            var query2 = query
                .Select(x => new ProductListModel
                {
                    ProductId = x.ProductId,
                    TaxSlabId = x.TaxSlabId,
                    Name = x.Name,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    RatingsCount = x.RatingsCount,
                    RatingsValue = x.RatingsValue,
                    ImagePath = x.ProductImages.OrderBy(o => o.ImageType).Take(1).FirstOrDefault().Path,
                    StockItems = x.StockGroups.OrderBy(o => o.SellPrice).Take(1).Select(m => new Models.StockGroups
                    {
                        StockGroupId = m.StockGroupId,
                        SizeId = m.SizeId,
                        BrandId = m.BrandId,
                        MaterialTypeId = m.MaterialTypeId,
                        SellPrice = m.SellPrice,
                        NetPrice = m.NetPrice,
                        Discount = m.Discount,
                    })
                   .FirstOrDefault(),
                    StockedQuantity = x.StockGroups.SelectMany(s => s.PurchaseOrderDetails).Sum(t => t.ReceivedQty),
                    SoldQuantity = x.StockGroups.SelectMany(s => s.SalesOrderDetails.Where(m => x.Status == (int)SalesOrderStatus.Completed)).Sum(t => t.OrderQty)
                })
                .AsNoTracking();
            var result = await query2.GetPaginateAsync(filter.PageNumber, filter.PageSize);

            return (result?.TotalCount, result?.Items?.ToList());
        }

        public async Task<Models.ProductDetails> GetByIdAsync(int productId)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Product.ProductDetails>()
                .Where(x => x.ProductId == productId)
               .AsNoTracking()
               .Select(x => new Models.ProductDetails
               {
                   TaxSlabId = x.TaxSlabId,
                   ProductId = x.ProductId,
                   Name = x.Name,
                   Hsn = x.Hsn,
                   CategoryId = x.CategoryId,
                   RatingsCount = x.RatingsCount,
                   RatingsValue = x.RatingsValue,
                   Description = x.Description,
                   MinimumStock = x.MinimumStock,
                   ProductImages = x.ProductImages.Select(i => new Models.ProductImages
                   {
                       ImageId = i.ImageId,
                       Path = i.Path
                   }).ToList()
               })
               .FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResponseResult> SaveAsync(Models.ProductDetails entity)
        {
            if (entity.ProductId > 0)
                return await UpdateAsync(entity);
            else
                return await CreateAsync(entity);
        }

        private async Task<ResponseResult> CreateAsync(Models.ProductDetails entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.ProductDetails)));

            try
            {
                var entityToAdd = _mapper.Map<Domain.Product.ProductDetails>(entity);

                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.Product.ProductDetails>().AnyAsync(x => x.CategoryId == entity.CategoryId && x.Name.ToLower().Equals(entity.Name.ToLower())))
                {
                    return new ResponseResult(2, Status.Error, string.Format(CommonConstants.ActionCommand_EntityAlreadyExistMessage, nameof(Models.ProductDetails)));
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                dbContext.Add(entityToAdd);

                await dbContext.SaveChangesAsync();
                scope.Complete();
                result.NewId = entityToAdd.ProductId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.ProductDetails)), ex);
            }

            return result;
        }

        private async Task<ResponseResult> UpdateAsync(Models.ProductDetails entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.ProductDetails)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.Product.ProductDetails>(entity.ProductId);

                if (entityToUpdate != null)
                {
                    entityToUpdate.CategoryId = entity.CategoryId;
                    entityToUpdate.Name = entity.Name;
                    entityToUpdate.Hsn = entity.Hsn;
                    entityToUpdate.MinimumStock = entity.MinimumStock;
                    entityToUpdate.RatingsCount = entity.RatingsCount;
                    entityToUpdate.RatingsValue = entity.RatingsValue;
                    entityToUpdate.Description = entity.Description;
                    dbContext.PartialUpdate(entityToUpdate);
                    await dbContext.SaveChangesAsync();
                }

                scope.Complete();
                result.NewId = entity.ProductId;

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.ProductCategory)), ex);
            }

            return result;
        }

        public async Task<(ResponseResult response, List<Models.ProductImages> images)> DeleteByIdAsync(int?[] productIds)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.ProductDetails)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                if (await dbContext.GetAsQuerable<Domain.Product.StockGroups>().AnyAsync(x => productIds.Contains(x.ProductId)))
                {
                    return (new ResponseResult(2, Status.Error, "Please delete the stock group before deleting the product."), null);
                }

                var images = await dbContext.GetAsQuerable<Domain.Product.ProductImages>()
                    .Where(x => productIds.Contains(x.ProductId))
                    .Select(x => new Models.ProductImages
                    {
                        ImageId = x.ImageId,
                        Path = x.Path
                    }).AsNoTracking().ToListAsync();

                if (images.Any())
                {
                    dbContext.RemoveByWhere<Domain.Product.ProductImages>(x => productIds.Contains(x.ProductId));
                }

                dbContext.RemoveByWhere<Domain.Product.ProductDetails>(x => productIds.Contains(x.ProductId));

                await dbContext.SaveChangesAsync();
                scope.Complete();

                return (response: result, images);
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.ProductDetails)), ex);
                result.Code = (int)Status.Error;
            }

            return (response: result, images: null);
        }

        public async Task<int?> GetCategoryIdAsync(int productId)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Product.ProductDetails>()
                .Where(x => x.ProductId == productId)
               .AsNoTracking()
               .Select(x => x.CategoryId)
               .FirstOrDefaultAsync();

            return result;
        }

        public async Task<(int? totalCount, List<ProductListModel> items)> GetProductsStatusAsync(ProductFilterViewModel filter)
        {
            using (var dbContext = _dbContext())
            {
                var query = dbContext.GetAsQuerable<Domain.Product.ProductDetails>()
                    .Include(x => x.StockGroups)
                   .AsQueryable();
                query = filter.CategoryId > 0 ? query.Where(x => x.CategoryId == filter.CategoryId) : query;
                //query = !filter.Name.IsNullOrEmpty() ? query.Where(x => x.Name.Contains(filter.Name)) : query;
                query = !filter.Name.IsNullOrEmpty() ? query.Where(x => EF.Functions.Like(x.Name, $"%{filter.Name}%")) : query;
                query = filter.StartPrice > 0 ? query.Where(x => x.StockGroups.Any(p => p.SellPrice > filter.StartPrice)) : query;
                query = filter.EndPrice > 0 ? query.Where(x => x.StockGroups.Any(p => p.SellPrice < filter.EndPrice)) : query;
                query = filter.SizeIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.SizeIds.Contains(p.SizeId))) : query;
                query = filter.BrandIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.BrandIds.Contains(p.BrandId))) : query;
                query = filter.ColorIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.ColorIds.Equals(p.ColorId))) : query;
                query = filter.GradeIds.HasItems() ? query.Where(x => x.StockGroups.Any(p => filter.GradeIds.Contains(p.GradeId))) : query;

                switch (filter.SortBy)
                {
                    case 1: //default
                        query = query.OrderBy(x => x.Name);
                        break;
                    case 2: // best match
                        query = query.OrderBy(x => x.StockGroups.OrderBy(o => o.Discount));
                        break;
                    case 3: // lowest price
                        query = query.OrderBy(x => x.StockGroups.OrderBy(o => o.SellPrice));
                        break;
                    case 4: // highest price
                        query = query.OrderByDescending(x => x.StockGroups.OrderByDescending(o => o.SellPrice));
                        break;
                }

                var result = await query
                    .Select(x => new ProductListModel
                    {
                        ProductId = x.ProductId,
                        Name = x.Name,
                        CategoryId = x.CategoryId,
                        CategoryName = x.Category.Name,
                        StockItems = x.StockGroups.OrderBy(o => o.SellPrice).Take(1).Select(m => new Models.StockGroups
                        {
                            StockGroupId = m.StockGroupId,
                            SizeId = m.SizeId,
                            BrandId = m.BrandId,
                            NetPrice = m.NetPrice,
                            Mrp = m.Mrp,
                            Discount = m.Discount,
                            SellPrice = m.SellPrice
                        })
                       .FirstOrDefault(),
                        StockedQuantity = x.StockGroups.Sum(s => s.PurchaseOrderDetails.Sum(t => t.ReceivedQty)),
                        SoldQuantity = x.StockGroups.Sum(s => s.SalesOrderDetails.Where(m => x.Status == (int)SalesOrderStatus.Completed).Sum(t => t.OrderQty))
                    })
                    .AsNoTracking()
                    .GetPaginateAsync(filter.PageNumber, filter.PageSize);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, result?.Items?.ToList());
                }
            }

            return (0, null);
        }

        public async Task<object> GetProductStatusAsync(int productId)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.GetAsQuerable<Domain.Product.StockGroups>()
                .Where(x => x.ProductId == productId)
                .Include(x => x.PurchaseOrderDetails)
                .Include(x => x.SalesOrderDetails)
                .Select(m => new
                {
                    m.StockGroupId,
                    m.SizeId,
                    Size = new Models.Sizes
                    {
                        Name = m.Size.Name
                    },
                    m.BrandId,
                    Brand = new Models.Brands
                    {
                        Name = m.Brand.Name
                    },
                    m.MaterialTypeId,
                    MaterialType = new Models.MaterialTypes
                    {
                        Name = m.MaterialType.Name
                    },
                    m.GradeId,
                    Grade = new Models.Grades
                    {
                        Name = m.Grade.Name
                    },
                    Color = new Models.Colors
                    {
                        Name = m.Color.Name
                    },
                    m.NetPrice,
                    m.Mrp,
                    m.Discount,
                    m.SellPrice,
                    StockedQuantity = m.PurchaseOrderDetails.Sum(t => t.ReceivedQty),
                    SoldQuantity = m.SalesOrderDetails.Where(x => x.Status == (int)SalesOrderStatus.Completed).Sum(t => t.OrderQty)
                })
               .AsQueryable()
               .ToListAsync();

            return result;
        }
    }
}
