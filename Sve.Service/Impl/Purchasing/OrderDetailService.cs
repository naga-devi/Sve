namespace Sve.Service.Impl.Purchasing
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Purchasing;
    using Sve.Service.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Models = Contract.Models.Purchasing;
    using ProductModel = Contract.Models.Product;

    public class OrderDetailService : IOrderDetailService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public OrderDetailService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<List<ProductModel.StockGroups>> GetByOrderId(int purchaseOrderId, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Product.StockGroups>()
                .Where(x => x.PurchaseOrderDetails.Any(m => m.PurchaseOrderId == purchaseOrderId))
                .Select(x => new ProductModel.StockGroups
                {
                    ProductId = x.ProductId,
                    Product = new ProductModel.ProductDetails
                    {
                        TaxSlabId = x.Product.TaxSlabId,
                        TaxSlab = new ProductModel.TaxSlabs
                        {
                            TotalTax = x.Product.TaxSlab.TotalTax,
                            Sgst = x.Product.TaxSlab.Sgst,
                            Cgst = x.Product.TaxSlab.Cgst,
                        },
                        Name = x.Product.Name,
                        Category = new ProductModel.ProductCategory
                        {
                            Name = x.Product.Category.Name
                        }
                    },
                    MaterialType = new ProductModel.MaterialTypes
                    {
                        Name = x.MaterialType.Name
                    },
                    Size = new ProductModel.Sizes
                    {
                        Name = x.Size.Name
                    },
                    Brand = new ProductModel.Brands
                    {
                        Name = x.Brand.Name
                    },
                    Color = new ProductModel.Colors
                    {
                        Name = x.Color.Name
                    },
                    Grade = new ProductModel.Grades
                    {
                        Name = x.Grade.Name
                    },
                    PurchaseOrderDetails = x.PurchaseOrderDetails.Select(p => new Models.PurchaseOrderDetail
                    {
                        Quanitity = p.Quanitity,
                        UnitPrice = p.UnitPrice,
                        Discount = p.Discount,
                        CgstAmount = p.CgstAmount,
                        SgstAmount = p.SgstAmount,
                        IgstAmount = p.IgstAmount,
                        Mrp = p.Mrp,
                        UnitMeasureId = p.UnitMeasureId,
                        UnitMeasure = new ProductModel.UnitMeasure
                        {
                            Name = p.UnitMeasure.Name
                        }
                    }).ToList()
                });

            //var sql = query.ToSql();

            return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        }

        //public async Task<Models.PurchaseOrderDetail> GetByIdAsync(int id, bool? disbaleTracking = false, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    using (var dbContext = _dbContext())
        //    {
        //        var query = dbContext.GetAsQuerable<Domain.PurchaseOrderDetail>().Where(x => x.Id == id);
        //        query = (bool)disbaleTracking ? query.AsNoTracking() : query;

        //        var result = await query.FirstOrDefaultAsync(cancellationToken);

        //        return _mapper.Map<Models.PurchaseOrderDetail>(result);
        //    }
        //}

        //public async Task<ResponseResult> CreateAsync(Models.PurchaseOrderDetail entity, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.PurchaseOrderDetail)));

        //    try
        //    {
        //        var entityToAdd = _mapper.Map<Domain.PurchaseOrderDetail>(entity);

        //        using (var dbContext = _dbContext())
        //        {
        //            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //            {
        //                dbContext.Add(entityToAdd);

        //                await dbContext.SaveChangesAsync(cancellationToken);
        //                scope.Complete();
        //                result.NewId = entityToAdd.Id;

        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.PurchaseOrderDetail)), ex);
        //    }

        //    return result;
        //}

        //public async Task<ResponseResult> UpdateAsync(Models.PurchaseOrderDetail entity, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_UpdateSuccessMessage, nameof(Models.PurchaseOrderDetail)));

        //    try
        //    {
        //        using (var dbContext = _dbContext())
        //        {
        //            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //            {
        //                var entityToUpdate = await dbContext.FindByIdAsync<Domain.PurchaseOrderDetail>(entity.Id);

        //                if (entityToUpdate != null)
        //                {
        //                    entityToUpdate.PurchaseOrderId = entity.PurchaseOrderId;
        //                    entityToUpdate.StockGroupId = entity.StockGroupId;
        //                    entityToUpdate.UnitPrice = entity.UnitPrice;
        //                    entityToUpdate.Quanitity = entity.Quanitity;
        //                    dbContext.Update(entityToUpdate);
        //                    await dbContext.SaveChangesAsync(cancellationToken);
        //                }

        //                scope.Complete();

        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_UpdateErrorMessage, nameof(Models.PurchaseOrderDetail)), ex);
        //    }

        //    return result;
        //}

        //public async Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.PurchaseOrderDetail)));

        //    try
        //    {
        //        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        {
        //            using (var dbContext = _dbContext())
        //            {
        //                dbContext.RemoveByWhere<Domain.Purchasing.PurchaseOrderDetail>(x => ids.Contains(x.Id));

        //                await dbContext.SaveChangesAsync(cancellationToken);
        //                scope.Complete();

        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.PurchaseOrderDetail)), ex);
        //    }

        //    return result;
        //}
    }
}
