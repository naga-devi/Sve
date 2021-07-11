using JxNet.Core;
using JxNet.Core.Extensions;
using JxNet.Extensions.ApiBase;
using JxNet.Extensions.WebHost.Models;
using Microsoft.AspNetCore.Mvc;
using Sve.Contract;
using Sve.Contract.Interface.Product;
using Sve.Contract.Interface.Sales;
using Sve.Contract.Models.Product;
using Sve.Contract.Models.Sales;
using Sve.Contract.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sve.WebHost.Controllers
{
    [Route("api/v1/cart")]
    [ApiController]
    public class ProductCartController : BaseController
    {
        private readonly IProductCategoryService _categoryService;
        private readonly IProductCartService _cartsService;
        private readonly IStockGroupsService _stockService;
        private readonly IOrderHeaderService _salesOrder;
        private readonly AppSettings _appSettings;

        public ProductCartController(
            IProductCategoryService categoryService,
            IProductCartService cartService,
            IStockGroupsService stockService,
            AppSettings appSettings,
            IOrderHeaderService salesOrder)
        {
            _cartsService = cartService;
            _stockService = stockService;
            _categoryService = categoryService;
            _salesOrder = salesOrder;
            _appSettings = appSettings;
        }

        [HttpPost]
        [Route("search")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GlobalSearch([FromBody] ProductFilterViewModel queryParameters)
        {
            var (totalCount, items) = await _cartsService.GetProductsAsync(queryParameters);

            return Ok(value: new
            {
                totalCount,
                items = items?.Select(x => new
                {
                    Id = x.StockItems?.StockGroupId ?? new Random().Next(),
                    ProductBaseId = x.ProductId,
                    x.Name,
                    x.RatingsCount,
                    x.RatingsValue,
                    x.Description,
                    //Images = new List<Images> {
                    //    new Images {
                    //        Small =x.ImagePath,
                    //        Big =x.ImagePath,
                    //        Medium =x.ImagePath
                    //    },
                    //},
                    //x.ImagePath,
                    ImagePath = _appSettings.ApplicationUrl.FixUrl(x.ImagePath),
                    x.StockItems?.NewPrice,
                    UnitPrice = x.StockItems?.NetPrice,
                    x.StockItems?.NetPrice,
                    x.StockItems?.OldPrice,
                    x.StockItems?.Cgst,
                    x.StockItems?.Sgst,
                    x.StockItems?.Discount,
                    x.StockItems?.BrandId,
                    x.StockItems?.MaterialTypeId,
                    x.StockItems?.SizeId,
                    x.StockItems?.ColorId,
                    x.StockItems?.GradeId,
                    x.AvailibilityCount,
                    x.CategoryId,
                    x.TaxSlabId,
                    x.CategoryName,
                    TotalAvailibilityCount = x.AvailibilityCount,
                })
            });
        }

        [HttpGet]
        [Route("view/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ViewProduct(int id)
        {
            var item = await _cartsService.GetProductByIdAsync(id);

            if (item == null)
                BadRequest("Invalid request");


            return Ok(value: new
            {
                Id = item.StockItems?.StockGroupId,
                ProductBaseId = item.ProductId,
                item.Name,
                item.RatingsCount,
                item.RatingsValue,
                item.Description,
                ImagePath = _appSettings.ApplicationUrl.FixUrl(item.ImagePath),
                Images = item.Images.Select(i => new Images
                {
                    Small = _appSettings.ApplicationUrl.FixUrl(i.Path),
                    Big = _appSettings.ApplicationUrl.FixUrl(i.Path),
                    Medium = _appSettings.ApplicationUrl.FixUrl(i.Path)
                }),
                //Images =  new List<Images> {
                //        new Images {
                //            Small =  _appSettings.ApplicationUrl.FixUrl(item.ImagePath),
                //            Big =_appSettings.ApplicationUrl.FixUrl(item.ImagePath),
                //            Medium =_appSettings.ApplicationUrl.FixUrl(item.ImagePath)
                //        },
                //    },
                item.StockItems?.NewPrice,
                item.StockItems?.Discount,
                item.StockItems?.OldPrice,
                UnitPrice = item.StockItems?.NetPrice,
                item.StockItems?.Cgst,
                item.StockItems?.Sgst,
                AvailibilityCount = item?.StockItems?.PurchaseOrderDetails.Sum(t => t.StockedQty) - item?.StockItems?.SalesOrderDetails.Sum(t => t.SoldQty),
                TotalAvailibilityCount = item.AvailibilityCount,
                //AvailibilityCount = x.StockedQuantity ?? 0 - x.SoldQuantity ?? 0,
                item.CategoryId,
                item.TaxSlabId,
                TaxSlab = new
                {
                    item.TaxSlab?.TotalTax,
                    item.TaxSlab?.Sgst,
                    item.TaxSlab?.Cgst,
                },
                item.StockItems?.SizeId,
                Sizes = item.Sizes?.GroupBy(g=> g.SizeId).Select(g => g.FirstOrDefault())?.Select(x => new
                {
                    name = x.Name,
                    id = x.SizeId,
                    selected = item.StockItems?.SizeId == x.SizeId
                })?.ToList(),
                item.StockItems?.BrandId,
                Brands = item.Brands?.GroupBy(g => g.BrandId).Select(g => g.FirstOrDefault())?.Select(x => new
                {
                    name = x.Name,
                    id = x.BrandId,
                    selected = item.StockItems?.BrandId == x.BrandId
                })?.ToList(),
                item.StockItems?.MaterialTypeId,
                MaterialTypes = item.MaterialTypes?.GroupBy(g => g.MaterialTypeId).Select(g => g.FirstOrDefault())?.Select(x => new
                {
                    name = x.Name,
                    id = x.MaterialTypeId,
                    selected = item.StockItems?.MaterialTypeId == x.MaterialTypeId
                })?.ToList(),
                item.StockItems?.GradeId,
                Grades = item.Grades?.GroupBy(g => g.GradeId).Select(g => g.FirstOrDefault())?.Select(x => new
                {
                    name = x.Name,
                    id = x.GradeId,
                    selected = item.StockItems?.GradeId == x.GradeId
                })?.ToList(),
                item.StockItems?.ColorId,
                Colors = item.Colors?.GroupBy(g => g.ColorId).Select(g => g.FirstOrDefault())?.Select(x => new
                {
                    name = x.Name,
                    id = x.ColorId,
                    selected = item.StockItems?.ColorId == x.ColorId
                })?.ToList(),
            });
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [Route("product-price/{productId:int}/{sizeId:int}/{brandId:int}/{materialTypeId:int}/{gradeId:int}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductPrice(int productId, int sizeId, int brandId, int materialTypeId, int gradeId)
        {
            var item = await _cartsService.GetProductPriceAsync(productId, sizeId, brandId, materialTypeId, gradeId);

            if (item == null)
            {
                return Ok(value: new
                {
                    Id = 0,
                    NewPrice = 0,
                    Discount = 0,
                    OldPrice = 0,
                    unitPrice = 0,
                    Cgst = 0,
                    Sgst = 0,
                    AvailibilityCount = 0,
                });
            }

            return Ok(value: new
            {
                Id = item.StockGroupId,
                item.NewPrice,
                item.Discount,
                item.OldPrice,
                item.Cgst,
                item.Sgst,
                unitPrice = item.NetPrice,
                AvailibilityCount = item.PurchaseOrderDetails.Sum(t => t.StockedQty) - item.SalesOrderDetails.Sum(t => t.SoldQty),
            });
        }

        [HttpGet]
        [Route("filter-types/{categoryId:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFilterTypesByCategory(int categoryId)
        {
            var items = await _categoryService.GetCacheAllAsync();

            var taxItems = await _categoryService.GetCacheTaxSlabsAsync();

            if (items == null)
                BadRequest("Invalid request");

            if (categoryId > 0)
            {
                items = items.Where(x => x.CategoryId == categoryId).ToList();
            }

            var productSizes = new List<Sizes>();
            items?.ToList()?.ForEach(x =>
            {
                productSizes.AddRange(x.ProductSizes);
            });

            var productBrands = new List<Brands>();
            items?.ToList()?.ForEach(x =>
            {
                productBrands.AddRange(x.ProductBrands);
            });

            var materialTypes = new List<MaterialTypes>();
            items?.ToList()?.ForEach(x =>
            {
                materialTypes.AddRange(x.ProductMaterialType);
            });

            var colors = new List<Colors>();
            items?.ToList()?.ForEach(x =>
            {
                colors.AddRange(x.ProductColors);
            });

            var grades = new List<Grades>();
            items?.ToList()?.ForEach(x =>
            {
                grades.AddRange(x.ProductGrades);
            });

            return Ok(value: new
            {
                categories = items?.Select(x => new { x.Name, Id = x.CategoryId, x.HasSubCategory, x.ParentId }).ToList(),
                materialTypes = materialTypes?.ToList()?.Distinct().Select(s => new { s.MaterialTypeId, s.Name, s.CategoryId, Selected = false }).ToList(),
                sizes = productSizes?.ToList()?.Distinct().Select(s => new { s.SizeId, s.Name, s.CategoryId, Selected = false }).ToList(),
                brands = productBrands?.ToList()?.Distinct()?.Select(s => new { s.BrandId, s.Name, s.CategoryId, Selected = false }).ToList(),
                taxItems = taxItems?.Select(x => new { x.TaxSlabId, x.Name, x.TotalTax, x.Cgst, x.Sgst }).ToList() ?? null,
                colors = colors?.Select(x => new { x.ColorId, x.Name, Selected = false, x.CategoryId }).ToList() ?? null,
                grades = grades?.Select(x => new { x.GradeId, x.Name, Selected = false, x.CategoryId }).ToList() ?? null
            });
        }

        [HttpPost]
        [Route("place-order")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PlaceOrder([FromBody] List<CheckOutModel> checkOutModels)
        {
            if (!checkOutModels.HasItems())
            {
                return BadRequest("Invalid request. Please contact administrator.");
            }
            //var taxSlabs = await _categoryService.GetCacheTaxSlabsAsync();
            var orderHeader = new SalesOrderHeader
            {
                OrderDate = DateTime.Now,
                TotalQuantity = checkOutModels.Sum(x => x.Quantity),
                TotalAmount = checkOutModels.Sum(x => x.TotalAmount),
                DiscountPercentage = 0,
                NetAmount = checkOutModels.Sum(x => x.TotalAmount),
                Freight = 0,
                RoundOffAmount = 0,
                GrandTotal = checkOutModels.Sum(x => x.TotalAmount),
                PaidAmount = 0,
                Status = (int)SalesOrderStatus.Pending
            };


            checkOutModels.ForEach(x =>
            {
                orderHeader.OrderDetails.Add(new SalesOrderDetails
                {
                    StockGroupId = x.Id,
                    OrderQty = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    CgstAmount = x.Cgst,
                    SgstAmount = x.Sgst,
                    IgstAmount = 0,
                    UnitMeasureId = 1,// default
                    Status = (int)SalesOrderStatus.Pending
                });
            });

            var response = await _salesOrder.PlaceOrderAsync(orderHeader);

            return Ok(response);
        }
    }
}