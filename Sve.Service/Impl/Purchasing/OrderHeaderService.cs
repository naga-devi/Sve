namespace Sve.Service.Impl.Purchasing
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract.Interface.Purchasing;
    using Sve.Service.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using Models = Contract.Models.Purchasing;
    using Domain = Domain.Purchasing;
    using ProductDomain = Domain.Product;
    using Sve.Contract.ViewModels;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class OrderHeaderService : IOrderHeaderService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public OrderHeaderService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<(int? totalCount, List<Models.PurchaseOrderHeader> items)> GetByExpressionAsync(int index, int size, string sortColumn, bool isDescending, Models.PurchaseOrderHeader filter = null, CancellationToken cancellationToken = default)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.PurchaseOrderHeader>();

            query = !filter.InvoiceNo.IsNullOrEmpty() ? query.Where(x => x.InvoiceNo == filter.InvoiceNo) : query;

            var result = await query
            .Select(x => new Models.PurchaseOrderHeader
            {
                PurchaseOrderId = x.PurchaseOrderId,
                PurchaseDate = x.PurchaseDate,
                InvoiceNo = x.InvoiceNo,
                TotalAmount = x.TotalAmount,
                DiscountAmount = x.DiscountAmount,
                NetAmount = x.NetAmount,
                CgstAmount = x.CgstAmount,
                SgstAmount = x.SgstAmount,
                IgstAmount = x.IgstAmount,
                GrandTotal = x.GrandTotal,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn.Value,
                VendorId = x.VendorId,
                Vendor = new Models.Vendors
                {
                    CompanyName = x.Vendor.CompanyName,
                    PhoneNo = x.Vendor.PhoneNo
                },
                PurchaseOrderDetail = x.OrderDetails.Select(m => new Models.PurchaseOrderDetail { StockedQty = m.StockedQty }).ToList()
            })
            .AsNoTracking()
            .GetPaginateAsync(index, size, sortColumn, isDescending, cancellationToken);

            return (result?.TotalCount, result?.Items?.ToList());
        }

        public async Task<Models.PurchaseOrderHeader> GetById(int purchaseOrderId)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.PurchaseOrderHeader>().Where(x => x.PurchaseOrderId == purchaseOrderId);

            var result = await query
            .Select(x => new Models.PurchaseOrderHeader
            {
                PurchaseOrderId = x.PurchaseOrderId,
                PurchaseDate = x.PurchaseDate,
                InvoiceNo = x.InvoiceNo,
                TotalAmount = x.TotalAmount,
                DiscountAmount = x.DiscountAmount,
                NetAmount = x.NetAmount,
                RoundOffAmount = x.RoundOffAmount,
                SubTotal = x.SubTotal,
                CgstAmount = x.CgstAmount,
                SgstAmount = x.SgstAmount,
                IgstAmount = x.IgstAmount,
                GrandTotal = x.GrandTotal,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn.Value,
                VendorId = x.VendorId,
                Vendor = new Models.Vendors
                {
                    CompanyName = x.Vendor.CompanyName,
                    PhoneNo = x.Vendor.PhoneNo,
                    Email = x.Vendor.Email,
                    TinNo = x.Vendor.TinNo,
                    Address = x.Vendor.Address
                },
                PurchaseOrderDetail = x.OrderDetails.Select(p => new Models.PurchaseOrderDetail
                {
                    Id = p.Id,
                    ReceivedQty = p.ReceivedQty,
                    RejectedQty = p.RejectedQty,
                    StockedQty = p.StockedQty,
                    UnitPrice = p.UnitPrice,
                    Discount = p.Discount,
                    CgstAmount = p.CgstAmount,
                    SgstAmount = p.SgstAmount,
                    IgstAmount = p.IgstAmount,
                    Mrp = p.Mrp,
                    UnitMeasureId = p.UnitMeasureId,
                    UnitMeasure = new Contract.Models.Product.UnitMeasure
                    {
                        Name = p.UnitMeasure.Name
                    },
                    StockGroup = new Contract.Models.Product.StockGroups
                    {
                        ProductId = p.StockGroup.ProductId,
                        Product = new Contract.Models.Product.ProductDetails
                        {
                            TaxSlabId = p.StockGroup.Product.TaxSlabId,
                            CategoryId = p.StockGroup.Product.CategoryId
                        },
                        MaterialTypeId = p.StockGroup.MaterialTypeId,
                        SizeId = p.StockGroup.SizeId,
                        BrandId = p.StockGroup.BrandId,
                        ColorId = p.StockGroup.ColorId,
                        GradeId = p.StockGroup.GradeId,
                    }
                }).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

            return result;
        }

        public async Task<ResponseResult> SaveAsync(PurchaseOrderCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Vendor.PurchaseOrderId > 0)
                return await UpdateAsync(request, cancellationToken);

            return await CreateAsync(request, cancellationToken);
        }

        private async Task<ResponseResult> CreateAsync(PurchaseOrderCreateRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.PurchaseOrderHeader)));

            try
            {
                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                //get existing stock items
                var searchProductIds = request.Purchases.Select(x => x.ProductId).ToList();
                var existingStockItems = await dbContext.GetAsQuerable<ProductDomain.StockGroups>()
                    .Where(x => searchProductIds.Contains(x.ProductId))
                    .AsNoTracking()
                    .Select(x => new Contract.Models.Product.StockGroups
                    {
                        StockGroupId = x.StockGroupId,
                        SizeId = x.SizeId,
                        BrandId = x.BrandId,
                        ProductId = x.ProductId,
                        MaterialTypeId = x.MaterialTypeId,
                        ColorId = x.ColorId,
                        GradeId = x.GradeId
                        //Product = new Contract.Models.Product.ProductDetails
                        //{
                        //    TaxSlabId = x.Product.TaxSlabId,
                        //    TaxSlab = new Contract.Models.Product.TaxSlabs
                        //    {
                        //        TotalTax = x.Product.TaxSlab.TotalTax,
                        //        Sgst = x.Product.TaxSlab.Sgst,
                        //        Cgst = x.Product.TaxSlab.Cgst,
                        //    }
                        //}
                    })
                    .ToListAsync(cancellationToken: cancellationToken);

                // set StockGroupId
                if (request.Purchases.HasItems())
                {
                    request.Purchases.ForEach(x =>
                    {
                        x.StockGroupId = existingStockItems
                        .FirstOrDefault(m => x.ProductId == m.ProductId && x.MaterialTypeId == m.MaterialTypeId
                            && x.SizeId == m.SizeId && x.BrandId == m.BrandId && x.GradeId == m.GradeId/* && x.ColorId == m.ColorId*/)?.StockGroupId ?? 0;
                    });
                }

                //get products and tax slabs
                //var requestedProductTaxSlabs = await dbContext.GetAsQuerable<ProductDomain.ProductDetails>()
                //    .Where(x => searchProductIds.Contains(x.ProductId))
                //    .Select(x => new ProductDomain.ProductDetails
                //    {
                //        ProductId = x.ProductId,
                //        TaxSlabId = x.TaxSlabId,
                //        TaxSlab = new ProductDomain.ProductTaxSlabs
                //        {
                //            Sgst = x.TaxSlab.Sgst,
                //            Cgst = x.TaxSlab.Cgst,
                //        }
                //    })
                //    .AsNoTracking()
                //    .ToListAsync();

                //create vendor if not exists
                if (request.Vendor.VendorId == 0)
                {
                    var vendor = new Domain.Vendors
                    {
                        CompanyName = request.Vendor.CompanyName,
                        TinNo = request.Vendor.TinNo,
                        Email = request.Vendor.Email,
                        PhoneNo = request.Vendor.PhoneNo,
                        Address = request.Vendor.Address,
                    };

                    dbContext.Add(vendor);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    request.Vendor.VendorId = vendor.VendorId;
                }

                //create  purchase header
                if (request.Vendor.VendorId > 0)
                {
                    var stocksToAdd = request.Purchases.Select(x => new ProductDomain.StockGroups
                    {
                        ProductId = x.ProductId,
                        StockGroupId = (int)x.StockGroupId,
                        MaterialTypeId = x.MaterialTypeId,
                        SizeId = x.SizeId,
                        BrandId = x.BrandId,
                        GradeId = x.GradeId,
                        ColorId = x.ColorId,
                        BasicMrp = x.Mrp,
                        NetPrice = x.UnitPrice,
                        Cgst = x.CgstAmount,
                        Sgst = x.CgstAmount,
                        TaxAmount = Math.Round(x.CgstAmount + x.SgstAmount, 2), //CalculateTax(requestedProductTaxSlabs, x.UnitPrice, x.ProductId)
                        Mrp = Math.Round(x.UnitPrice + x.CgstAmount + x.SgstAmount, 2),
                        Discount = 0,
                        SellPrice = Math.Round((decimal)GetSellPrice(x.UnitPrice, x.CgstAmount, x.SgstAmount, 0), 2),
                        IsPrime = x.IsPrime ?? false,
                        MinimumStock = x.MinimumStock ?? 1,
                        RatingsCount = x.RatingsCount ?? 1,
                        RatingsValue = x.RatingsValue ?? 1,
                        Description = x.Description,
                        Status = (int)EntityStatus.Active
                    }).ToList();

                    if (stocksToAdd.Any(x => x.StockGroupId == 0))
                    {
                        if (stocksToAdd.HasItems())
                        {
                            var stocksToAddModel = stocksToAdd.Where(x => x.StockGroupId == 0).ToList();

                            if (stocksToAddModel.HasItems())
                            {
                                dbContext.AddRange(stocksToAddModel.ToArray());
                                await dbContext.SaveChangesAsync(cancellationToken);
                            }
                        }
                    }

                    //Tax and grandtotal calculations

                    //var TotalAmount = request.Purchases.Sum(x => x.UnitPrice * x.Quanitity);
                    //var netAmount = request.Vendor.TotalAmount - request.Vendor.Discount;
                    //var cgstAmount = request.Vendor.CgstInPercentage > 0 ? netAmount - (netAmount / request.Vendor.CgstInPercentage) * 100 : 0;
                    //var sgstAmount = request.Vendor.SgstInPercentage > 0 ? netAmount - (netAmount / request.Vendor.SgstInPercentage) * 100 : 0;
                    //var igstAmount = request.Vendor.IgstInPercentage > 0 ? netAmount - (netAmount / request.Vendor.IgstInPercentage) * 100 : 0;
                    var roundOffAmount = request.Vendor.RoundOffAmount ?? 0;

                    var purchaseOrderHeaderToAdd = new Domain.PurchaseOrderHeader
                    {
                        PurchaseDate = request.Vendor.PurchaseDate.Value,
                        InvoiceNo = request.Vendor.InvoiceNo,
                        VendorId = request.Vendor.VendorId,
                        TotalAmount = request.Vendor.TotalAmount,
                        DiscountAmount = request.Vendor.Discount,
                        NetAmount = request.Vendor.NetAmount,
                        //CgstInPercentage = request.Vendor.CgstInPercentage,
                        CgstAmount = request.Vendor.CgstAmount,
                        //IgstInPercentage = request.Vendor.IgstInPercentage,
                        IgstAmount = request?.Vendor?.IgstAmount ?? 0,
                        //SgstInPercentage = request.Vendor.SgstInPercentage,
                        SgstAmount = request.Vendor.SgstAmount,
                        RoundOffAmount = roundOffAmount,
                        Freight = request.Vendor.Freight ?? 0,
                        SubTotal = request.Vendor.GrandTotal + roundOffAmount,
                        GrandTotal = request.Vendor.GrandTotal,
                        Status = (int)EntityStatus.Active
                    };

                    dbContext.Add(purchaseOrderHeaderToAdd);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    if (purchaseOrderHeaderToAdd.PurchaseOrderId > 0)
                    {
                        //adding to purchase order details
                        var purchases = request.Purchases.Select(x => new Domain.PurchaseOrderDetail
                        {
                            PurchaseOrderId = purchaseOrderHeaderToAdd.PurchaseOrderId,
                            StockGroupId = (int)stocksToAdd.FirstOrDefault(m => x.ProductId == m.ProductId && x.MaterialTypeId == m.MaterialTypeId && x.SizeId == m.SizeId && x.BrandId == m.BrandId && x.GradeId == m.GradeId)?.StockGroupId,
                            ReceivedQty = x.ReceivedQty,
                            Mrp = x.Mrp,
                            UnitPrice = x.UnitPrice,
                            Discount = x.Discount ?? 0,
                            CgstAmount = x.CgstAmount,
                            SgstAmount = x.SgstAmount,
                            IgstAmount = 0,
                            UnitMeasureId = x.UnitMeasureId,
                            Status = (int)EntityStatus.Active
                        }).ToList();

                        //purchases.ForEach(purchase =>
                        //{
                        //    dbContext.Add(purchase);
                        //});

                        if (purchases.HasItems())
                            dbContext.AddRange(purchases.ToArray());

                        await dbContext.SaveChangesAsync(cancellationToken);
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.PurchaseOrderHeader)), ex);
                result.Code = (int)Status.Error;
            }

            return result;
        }

        private async Task<ResponseResult> UpdateAsync(PurchaseOrderCreateRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_SaveSuccessMessage, nameof(Models.PurchaseOrderHeader)));

            try
            {
                //set default values

                request?.Purchases?.ForEach(x =>
                {
                    x.Id = x.Id == null ? 0 : x.Id;
                    x.StockGroupId = x.StockGroupId == null ? 0 : x.StockGroupId;
                });

                using var dbContext = _dbContext();
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var existingDetails = await dbContext.GetAsQuerable<Domain.PurchaseOrderDetail>().Where(x => x.PurchaseOrderId == request.Vendor.PurchaseOrderId).ToListAsync(cancellationToken: cancellationToken);
                var deleteIds = Array.Empty<int>();

                if (existingDetails != null && existingDetails.Any())
                {
                    var detailsToDeleted = existingDetails.Where(e => !request.Purchases.Any(n => n.Id == e.Id)).ToList();
                    deleteIds = detailsToDeleted.Select(x => x.Id).ToArray();

                    if (deleteIds.HasItems())
                        dbContext.RemoveByWhere<Domain.PurchaseOrderDetail>(x => deleteIds.Contains(x.Id));

                    request.Purchases = request.Purchases.Where(x => !deleteIds.Contains((int)x.Id)).ToList();
                }

                //get existing stock items
                if (request.Purchases != null && request.Purchases.Any(x => x.Id == 0 || x.StockGroupId == 0))
                {
                    var searchProductIds = request.Purchases.Where(x => x.Id == 0 || x.StockGroupId == 0).Select(x => x.ProductId).ToList();

                    var existingStockItems = await dbContext.GetAsQuerable<ProductDomain.StockGroups>()
                        .Where(x => searchProductIds.Contains(x.ProductId))
                        .AsNoTracking()
                        .Select(x => new Contract.Models.Product.StockGroups
                        {
                            StockGroupId = x.StockGroupId,
                            SizeId = x.SizeId,
                            BrandId = x.BrandId,
                            ProductId = x.ProductId,
                            MaterialTypeId = x.MaterialTypeId,
                            ColorId = x.ColorId,
                            GradeId = x.GradeId


                            //Product = new Contract.Models.Product.ProductDetails
                            //{
                            //    TaxSlabId = x.Product.TaxSlabId,
                            //    TaxSlab = new Contract.Models.Product.TaxSlabs
                            //    {
                            //        TotalTax = x.Product.TaxSlab.TotalTax,
                            //        Sgst = x.Product.TaxSlab.Sgst,
                            //        Cgst = x.Product.TaxSlab.Cgst,
                            //    }
                            //}
                        })
                        .ToListAsync(cancellationToken: cancellationToken);

                    // set StockGroupId

                    request.Purchases.Where(x => x.Id == 0 || x.StockGroupId == 0).ToList().ForEach(x =>
                    {
                        var stockGroupId = existingStockItems
                        .FirstOrDefault(m => x.ProductId == m.ProductId && x.MaterialTypeId == m.MaterialTypeId
                            && x.SizeId == m.SizeId && x.BrandId == m.BrandId && x.GradeId == m.GradeId)?.StockGroupId ?? 0;

                        if (stockGroupId > 0)
                        {
                            x.StockGroupId = stockGroupId;
                        }
                    });
                }

                //create vendor if not exists
                if (request.Vendor.VendorId == 0)
                {
                    var vendor = new Domain.Vendors
                    {
                        CompanyName = request.Vendor.CompanyName,
                        TinNo = request.Vendor.TinNo,
                        Email = request.Vendor.Email,
                        PhoneNo = request.Vendor.PhoneNo,
                        Address = request.Vendor.Address,
                    };

                    dbContext.Add(vendor);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    request.Vendor.VendorId = vendor.VendorId;
                }

                //create  purchase header
                if (request.Vendor.VendorId > 0)
                {
                    var stocksToAdd = request.Purchases.Where(x => x.Id == 0 || x.StockGroupId == 0).Select(x => new ProductDomain.StockGroups
                    {
                        ProductId = x.ProductId,
                        StockGroupId = (int)x.StockGroupId,
                        MaterialTypeId = x.MaterialTypeId,
                        SizeId = x.SizeId,
                        BrandId = x.BrandId,
                        GradeId = x.GradeId,
                        ColorId = x.ColorId,
                        BasicMrp = x.Mrp,
                        NetPrice = x.UnitPrice,
                        Cgst = x.CgstAmount,
                        Sgst = x.CgstAmount,
                        TaxAmount = Math.Round(x.CgstAmount + x.SgstAmount, 2), //CalculateTax(requestedProductTaxSlabs, x.UnitPrice, x.ProductId)
                        Mrp = Math.Round(x.UnitPrice + x.CgstAmount + x.SgstAmount, 2),
                        Discount = 0,
                        SellPrice = Math.Round((decimal)GetSellPrice(x.UnitPrice, x.CgstAmount, x.SgstAmount, 0), 2),

                        IsPrime = x.IsPrime ?? false,
                        MinimumStock = x.MinimumStock ?? 1,
                        RatingsCount = x.RatingsCount ?? 1,
                        RatingsValue = x.RatingsValue ?? 1,
                        Description = x.Description,
                        Status = (int)EntityStatus.Active
                    }).ToList();

                    if (stocksToAdd.Any(x => x.StockGroupId == 0))
                    {
                        var stockToAddModel = stocksToAdd.Where(x => x.StockGroupId == 0).ToList().ToArray();
                        if (stockToAddModel.HasItems())
                        {
                            dbContext.AddRange(stockToAddModel);
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }

                    var roundOffAmount = request.Vendor.RoundOffAmount ?? 0;

                    var orderToUpdate = await dbContext.GetAsQuerable<Domain.PurchaseOrderHeader>().Where(x => x.PurchaseOrderId == request.Vendor.PurchaseOrderId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

                    orderToUpdate.PurchaseDate = request.Vendor.PurchaseDate.Value;
                    orderToUpdate.InvoiceNo = request.Vendor.InvoiceNo;
                    orderToUpdate.VendorId = request.Vendor.VendorId;
                    orderToUpdate.TotalAmount = request.Vendor.TotalAmount;
                    orderToUpdate.DiscountAmount = request.Vendor.Discount;
                    orderToUpdate.NetAmount = request.Vendor.NetAmount;
                    //CgstInPercentage = request.Vendor.CgstInPercentage,
                    orderToUpdate.CgstAmount = request.Vendor.CgstAmount;
                    //IgstInPercentage = request.Vendor.IgstInPercentage,
                    orderToUpdate.IgstAmount = request?.Vendor?.IgstAmount ?? 0;
                    //SgstInPercentage = request.Vendor.SgstInPercentage,
                    orderToUpdate.SgstAmount = request.Vendor.SgstAmount;
                    orderToUpdate.RoundOffAmount = roundOffAmount;
                    orderToUpdate.Freight = request.Vendor.Freight ?? 0;
                    orderToUpdate.SubTotal = request.Vendor.GrandTotal + roundOffAmount;
                    orderToUpdate.GrandTotal = request.Vendor.GrandTotal;
                    dbContext.Update(orderToUpdate);

                    if (orderToUpdate.PurchaseOrderId > 0)
                    {
                        //adding to purchase order details
                        var detailsToAdd = request.Purchases.Where(x => x.Id == 0).Select(x => new Domain.PurchaseOrderDetail
                        {
                            PurchaseOrderId = orderToUpdate.PurchaseOrderId,
                            StockGroupId = (int)stocksToAdd.FirstOrDefault(m => x.ProductId == m.ProductId && x.BrandId == m.BrandId && x.MaterialTypeId == m.MaterialTypeId && x.SizeId == m.SizeId
                             && x.GradeId == m.GradeId)?.StockGroupId,
                            ReceivedQty = x.ReceivedQty,
                            Mrp = x.Mrp,
                            UnitPrice = x.UnitPrice,
                            Discount = x.Discount ?? 0,
                            CgstAmount = x.CgstAmount,
                            SgstAmount = x.SgstAmount,
                            IgstAmount = 0,
                            UnitMeasureId = x.UnitMeasureId,
                            Status = (int)EntityStatus.Active
                        }).ToList();

                        if (detailsToAdd.HasItems())
                        {
                            if (detailsToAdd.HasItems())
                                dbContext.AddRange(detailsToAdd.ToArray());
                        }

                        var detailsToUpdate = await dbContext.GetAsQuerable<Domain.PurchaseOrderDetail>().Where(x => x.PurchaseOrderId == request.Vendor.PurchaseOrderId).ToListAsync(cancellationToken: cancellationToken);

                        if (detailsToUpdate != null && detailsToUpdate.Any())
                        {
                            detailsToUpdate.ForEach(d =>
                            {
                                var detail = request.Purchases.FirstOrDefault(x => x.Id == d.Id);
                                if (detail != null)
                                {
                                    d.ReceivedQty = detail.ReceivedQty;
                                    d.Mrp = detail.Mrp;
                                    d.UnitPrice = detail.UnitPrice;
                                    d.Discount = detail.Discount ?? 0;
                                    d.CgstAmount = detail.CgstAmount;
                                    d.SgstAmount = detail.SgstAmount;
                                    d.IgstAmount = 0;
                                    d.UnitMeasureId = detail.UnitMeasureId;
                                }
                            });

                            if (detailsToUpdate.HasItems())
                                dbContext.UpdateRange(detailsToUpdate.ToArray());
                        }

                        await dbContext.SaveChangesAsync(cancellationToken);
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_SaveErrorMessage, nameof(Models.PurchaseOrderHeader)), ex);
                result.Code = (int)Status.Error;
            }

            return result;
        }

        private static decimal? GetSellPrice(decimal netPrice, decimal cgst, decimal sgst, decimal discountPercentage)
        {
            var total = netPrice + cgst + sgst;

            return discountPercentage == 0 ? total : total - (total * discountPercentage) / 100;
        }

        public async Task<ResponseResult> DeleteByIdAsync(int[] ids, CancellationToken cancellationToken = default)
        {
            var result = new ResponseResult(Status.Success, string.Format(CommonConstants.ActionCommand_DeleteSuccessMessage, nameof(Models.PurchaseOrderDetail)));

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var dbContext = _dbContext();
                if (ids.HasItems())
                    dbContext.RemoveByWhere<Domain.PurchaseOrderDetail>(x => ids.Contains(x.PurchaseOrderId));

                if (ids.HasItems())
                    dbContext.RemoveByWhere<Domain.PurchaseOrderHeader>(x => ids.Contains(x.PurchaseOrderId));

                await dbContext.SaveChangesAsync(cancellationToken);
                scope.Complete();

                return result;
            }
            catch (Exception ex)
            {
                result = result.GetErrorResult(string.Format(CommonConstants.ActionCommand_DeleteErrorMessage, nameof(Models.PurchaseOrderDetail)), ex);
            }

            return result;
        }

        //private decimal? CalculateTax(List<ProductDomain.ProductDetails> productDetails, decimal netAmount, int? productId)
        //{
        //    var cgstPercent = productDetails.Where(t => t.ProductId == productId)?.FirstOrDefault()?.TaxSlab?.Cgst ?? 0;
        //    var sgstPercent = productDetails.Where(t => t.ProductId == productId)?.FirstOrDefault()?.TaxSlab?.Sgst ?? 0;

        //    var cgstAmount = cgstPercent / netAmount * 100;
        //    var sgstAmount = sgstPercent / netAmount * 100;

        //    return cgstAmount + sgstAmount + 0;
        //}
    }
}
