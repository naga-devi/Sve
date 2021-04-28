namespace Sve.Service.Impl.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading.Tasks;
    using System.Transactions;
    using AutoMapper;
    using JxNet.Core;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Product;
    using Sve.Service.Data;
    using Models = Contract.Models.Product;
    using PurchaseModels = Contract.Models.Purchasing;
    using JxNet.Core.Extensions;
    public class StockGroupsService : IStockGroupsService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public StockGroupsService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<(int? totalCount, List<Models.StockGroups> items)> GetByExpressionAsync(int productId, int index, int size, string sortColumn, bool isDescending, Models.StockGroups filter = null)
        {
            using (var dbContext = _dbContext())
            {
                var result = await dbContext.GetAsQuerable<Domain.Product.StockGroups>()
                    .Where(x => x.ProductId == productId)
                    .Select(x => new Models.StockGroups
                    {
                        MaterialTypeId = x.MaterialTypeId,
                        MaterialType = new Models.MaterialTypes
                        {
                            Name = x.MaterialType.Name
                        },
                        StockGroupId = x.StockGroupId,
                        NetPrice = x.NetPrice,
                        Mrp = x.Mrp,
                        Discount = x.Discount,
                        SellPrice = x.SellPrice,
                        BrandId = x.BrandId,
                        TaxAmount = x.TaxAmount,
                        Product = new Models.ProductDetails
                        {
                            TaxSlab = new Models.TaxSlabs
                            {
                                TotalTax = x.Product.TaxSlab.TotalTax,
                                Cgst = x.Product.TaxSlab.Cgst,
                                Sgst = x.Product.TaxSlab.Sgst,
                            }
                        },
                        Brand = new Models.Brands
                        {
                            Name = x.Brand.Name
                        },
                        SizeId = x.SizeId,
                        Size = new Models.Sizes
                        {
                            Name = x.Size.Name
                        },
                        PurchaseOrderDetails = x.PurchaseOrderDetails.Select(t => new PurchaseModels.PurchaseOrderDetail
                        {
                            Quanitity = t.Quanitity
                        }).ToList()
                    })
                    .AsNoTracking()
                    .GetPaginateAsync(index, size, sortColumn, isDescending);

                if ((bool)result?.Items?.HasItems())
                {
                    return (result?.TotalCount, _mapper.Map<List<Models.StockGroups>>(result?.Items?.ToList()));
                }
            }

            return (0, null);
        }

        public async Task<Models.StockGroups> GetByIdAsync(int stockGroupId)
        {
            using var dbContext = _dbContext();
            var result = await dbContext.FindByIdAsync<Domain.Product.StockGroups>(stockGroupId);

            return _mapper.Map<Models.StockGroups>(result);
        }

        //public async Task<ResponseResult> CreateAsync(Models.StockGroups entity, int stockedQty, decimal buyPrice)
        //{
        //    var result = new ResponseResult((int)Status.Success, Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.StockGroups)), null);

        //    try
        //    {
        //        result = await ValidateOnInsert(entity);

        //        if (result.Status == Status.Error)
        //        {
        //            return result;
        //        }

        //        var entityToAdd = _mapper.Map<Domain.Product.StockGroups>(entity);

        //        using (var dbContext = _dbContext())
        //        {
        //            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //            {
        //                dbContext.Add(entityToAdd);

        //                await dbContext.SaveChangesAsync();
        //                result.NewId = entityToAdd.StockGroupId;

        //                if (entityToAdd.StockGroupId > 0)
        //                {
        //                    var history = new Domain.Purchasing.PurchaseOrderDetail
        //                    {
        //                        Status = (int)EntityStatus.Active,
        //                        StockGroupId = entityToAdd.StockGroupId,
        //                        Quanitity = stockedQty,
        //                        UnitPrice = buyPrice,
        //                    };

        //                    dbContext.Add(history);

        //                    await dbContext.SaveChangesAsync();
        //                }

        //                scope.Complete();

        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.StockGroups)), ex);
        //    }

        //    return result;
        //}

        public async Task<ResponseResult> UpdatePriceAsync(Models.StockGroups entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.StockGroups)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var entityToUpdate = await dbContext.FindByIdAsync<Domain.Product.StockGroups>(entity.StockGroupId);

                if (entityToUpdate != null)
                {
                    //entityToUpdate.ProductId = entity.ProductId;
                    //entityToUpdate.ProductTypeId = entity.ProductTypeId;
                    //entityToUpdate.SizeId = entity.SizeId;
                    //entityToUpdate.BrandId = entity.BrandId;
                    entityToUpdate.NetPrice = entity.NetPrice;
                    entityToUpdate.TaxAmount = entity.TaxAmount ?? 0;
                    entityToUpdate.Mrp = entity.Mrp ?? 0;
                    entityToUpdate.Discount = entity.Discount ?? 0;
                    entityToUpdate.SellPrice = entity.SellPrice ?? 0;
                    dbContext.PartialUpdate(entityToUpdate);
                    await dbContext.SaveChangesAsync();
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

        private async Task<ResponseResult> ValidateOnInsert(Models.StockGroups entity)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.StockGroups)));

            using (var dbContext = _dbContext())
            {
                if (await dbContext.GetAsQuerable<Domain.Product.StockGroups>()
                    .AsNoTracking()
                    .AnyAsync(x => x.ProductId == entity.ProductId
                    && x.MaterialTypeId == entity.MaterialTypeId
                    && x.SizeId == entity.SizeId
                    && x.BrandId == entity.BrandId
                    && x.Status == (int)EntityStatus.Active))
                {
                    result = new ResponseResult((int)Status.Error, Status.Error, "Stock group already existed.", null);
                }
            }

            return result;
        }
    }
}
