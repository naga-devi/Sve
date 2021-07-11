namespace Sve.Service.Impl.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Core;
    using System.Threading.Tasks;
    using AutoMapper;
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Sve.Contract;
    using Sve.Contract.Interface.Product;
    using Sve.Contract.ViewModels;
    using Sve.Service.Data;
    using Models = Contract.Models.Product;
    using PurchasingModels = Contract.Models.Purchasing;
    using JxNet.Core;
    using JxNet.Core.Extensions;
    public class ProductCartService : IProductCartService
    {
        private readonly Func<ISveServiceDbContext> _dbContext;
        private readonly IMapper _mapper;

        public ProductCartService(Func<ISveServiceDbContext> cdrDbContext, IMapper mapper)
        {
            _dbContext = cdrDbContext;
            _mapper = mapper;
        }

        public async Task<(int? totalCount, List<ProductListModel> items)> GetProductsAsync(ProductFilterViewModel filter)
        {
            using var dbContext = _dbContext();
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

            var result = await query
                .Select(x => new ProductListModel
                {
                    TaxSlabId = x.TaxSlabId,
                    ProductId = x.ProductId,
                    Name = x.Name,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    RatingsCount = x.RatingsCount,
                    RatingsValue = x.RatingsValue,
                    Description = x.Description,
                    ImagePath = x.ProductImages.OrderBy(o => o.ImageType).Take(1).FirstOrDefault().Path,
                    StockItems = x.StockGroups.OrderBy(o => o.SellPrice).Take(1).Select(m => new Models.StockGroups
                    {
                        StockGroupId = m.StockGroupId,
                        SizeId = m.SizeId,
                        BrandId = m.BrandId,
                        MaterialTypeId = m.MaterialTypeId,
                        ColorId = m.ColorId,
                        GradeId = m.GradeId,
                        NetPrice = m.NetPrice,
                        Discount = m.Discount,
                        Cgst = m.Cgst,
                        Sgst = m.Sgst,
                        TaxAmount = m.TaxAmount,
                        Mrp = m.Mrp,
                        SellPrice = m.SellPrice,
                    })
                   .FirstOrDefault(),
                    StockedQuantity = x.StockGroups.SelectMany(s => s.PurchaseOrderDetails).Sum(t => t.StockedQty),
                    SoldQuantity = x.StockGroups.SelectMany(s => s.SalesOrderDetails.Where(m => x.Status == (int)SalesOrderStatus.Completed)).Sum(t => t.SoldQty)
                    //SoldQty = x.StockGroups.SelectMany(s => s.SalesOrderDetails.Where(m => x.Status == (int)SalesOrderStatus.Completed)).Sum(t => t.SoldQty)
                })
                .AsNoTracking()
                .GetPaginateAsync(filter.PageNumber, filter.PageSize);

            return (result?.TotalCount, result?.Items?.ToList());
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int productId)
        {
            using var dbContext = _dbContext();
            try
            {
                var query = dbContext.GetAsQuerable<Domain.Product.ProductDetails>()
                    .Where(x => x.ProductId == productId)
                   .AsNoTracking()
                   .Select(x => new ProductViewModel
                   {
                       ProductId = x.ProductId,
                       Name = x.Name,
                       CategoryId = x.CategoryId,
                       TaxSlabId = x.TaxSlabId,
                       TaxSlab = new Models.TaxSlabs
                       {
                           TotalTax = x.TaxSlab.TotalTax,
                           Sgst = x.TaxSlab.Sgst,
                           Cgst = x.TaxSlab.Cgst
                       },
                       RatingsCount = x.RatingsCount,
                       RatingsValue = x.RatingsValue,
                       Description = x.Description,
                       ImagePath = x.ProductImages.OrderBy(o => o.ImageType).Take(1).FirstOrDefault().Path,
                       Images = x.ProductImages.Select(i => new Models.ProductImages
                       {
                           Path = i.Path
                       }).ToList(),
                       StockItems = x.StockGroups.OrderBy(o => o.SellPrice).Take(1).Select(m => new Models.StockGroups
                       {
                           StockGroupId = m.StockGroupId,
                           MaterialTypeId = m.MaterialTypeId,
                           SizeId = m.SizeId,
                           BrandId = m.BrandId,
                           ColorId = m.ColorId,
                           GradeId = m.GradeId,
                           NetPrice = m.NetPrice,
                           Cgst = m.Cgst,
                           Sgst = m.Sgst,
                           TaxAmount = m.TaxAmount,
                           Mrp = m.Mrp,
                           Discount = m.Discount,
                           SellPrice = m.SellPrice,
                           PurchaseOrderDetails = m.PurchaseOrderDetails.Select(t => new PurchasingModels.PurchaseOrderDetail
                           {
                               StockedQty = t.StockedQty
                           }).ToList(),
                           SalesOrderDetails = m.SalesOrderDetails.Where(so => so.Status == (int)SalesOrderStatus.Completed).Select(t => new Contract.Models.Sales.SalesOrderDetails
                           {
                               SoldQty = t.SoldQty
                           }).ToList()
                       })
                       .FirstOrDefault(),

                       //TODO
                       Sizes = x.StockGroups.Where(g => g.SizeId == g.Size.SizeId).Select(t => new Models.Sizes { SizeId = t.SizeId, Name = t.Size.Name }).ToList(),
                       Brands = x.StockGroups.Where(g => g.BrandId == g.Brand.BrandId).Select(p => new Models.Brands { BrandId = p.BrandId, Name = p.Brand.Name }).ToList(),
                       MaterialTypes = x.StockGroups.Where(g => g.MaterialTypeId == g.MaterialType.MaterialTypeId).Select(p => new Models.MaterialTypes { MaterialTypeId = p.MaterialTypeId, Name = p.MaterialType.Name }).ToList(),
                       Colors = x.StockGroups.Where(g => g.ColorId == g.Color.ColorId).Select(p => new Models.Colors { ColorId = p.ColorId, Name = p.Color.Name }).ToList(),
                       Grades = x.StockGroups.Where(g => g.GradeId == g.Grade.GradeId).Select(p => new Models.Grades { GradeId = p.GradeId, Name = p.Grade.Name }).ToList(),

                       //Sizes = x.StockGroups.GroupBy(g => new { g.SizeId, g.Size.Name }).Select(t => new Models.Sizes { SizeId = t.Key.SizeId, Name = t.Key.Name }).OrderBy(o => o.Name).ToList(),
                       //Brands = x.StockGroups.GroupBy(g => g.BrandId).Select(p => p.First()).Select(p => new Models.Brands { BrandId = p.BrandId, Name = p.Brand.Name }).ToList(),
                       //MaterialTypes = x.StockGroups.GroupBy(g => g.MaterialTypeId).Select(p => p.First()).Select(p => new Models.MaterialTypes { MaterialTypeId = p.MaterialTypeId, Name = p.MaterialType.Name }).ToList(),
                       //Colors = x.StockGroups.GroupBy(g => g.ColorId).Select(p => p.First()).Select(p => new Models.Colors { ColorId = p.ColorId, Name = p.Color.Name }).ToList(),
                       //Grades = x.StockGroups.GroupBy(g => g.GradeId).Select(p => p.First()).Select(p => new Models.Grades { GradeId = p.GradeId, Name = p.Grade.Name }).ToList(),
                   });

                var sql = query.ToQueryString();
                var result = await query.FirstOrDefaultAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Models.StockGroups> GetProductPriceAsync(int productId, int sizeId, int brandId, int materialTypeId, int gradeId)
        {
            using var dbContext = _dbContext();
            var query = dbContext.GetAsQuerable<Domain.Product.StockGroups>()
                .Where(x => x.ProductId == productId && x.SizeId == sizeId && x.BrandId == brandId && x.MaterialTypeId == materialTypeId && x.GradeId == gradeId)
                .Select(x => new Models.StockGroups
                {
                    StockGroupId = x.StockGroupId,
                    NetPrice = x.NetPrice,
                    Mrp = x.Mrp,
                    Discount = x.Discount,
                    SellPrice = x.SellPrice,
                    Cgst = x.Cgst,
                    Sgst = x.Sgst,
                    PurchaseOrderDetails = x.PurchaseOrderDetails.Select(t => new PurchasingModels.PurchaseOrderDetail
                    {
                        StockedQty = t.StockedQty
                    }).ToList(),
                    SalesOrderDetails = x.SalesOrderDetails.Select(t => new Sve.Contract.Models.Sales.SalesOrderDetails
                    {
                        SoldQty = t.SoldQty
                    }).ToList()
                }).AsQueryable();
            var sql = query.ToSql();

            var result = await query.AsNoTracking()
                 .FirstOrDefaultAsync();

            return _mapper.Map<Models.StockGroups>(result);
        }
    }
}
