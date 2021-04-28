namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Product;
    using Sve.Contract.Models.Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JxNet.Extensions.WebHost.Models;

    [Authorize]
    [Route("api/product/{productId:int}/stockitems")]
    [ApiController]
    public class ProductStockItemsController : BaseController
    {
        private readonly IStockGroupsService _stockItemsService;
        private readonly IProductCategoryService _categoryService;
        private readonly IProductDetailsService _productDetails;

        public ProductStockItemsController
        (
            IStockGroupsService stockItemsService,
            IProductCategoryService categoryService,
            IProductDetailsService productDetails
        )
        {
            _stockItemsService = stockItemsService;
            _categoryService = categoryService;
            _productDetails = productDetails;
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllStockItems([FromRoute] int productId, [FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<StockGroups>();
            var (totalCount, items) = await _stockItemsService.GetByExpressionAsync(productId: productId, index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new
            {
                totalCount,
                items = items?.Select(x => new
                {
                    x.StockGroupId,
                    x.MaterialTypeId,
                    MaterialType = x.MaterialType.Name,
                    x.BrandId,
                    BrandName = x.Brand.Name,
                    x.SizeId,
                    SizeName = x.Size.Name,
                    x.NetPrice,
                    x.TaxAmount,
                    x.Mrp,
                    x.Discount,
                    x.SellPrice,
                    StockedQty = x.PurchaseOrderDetails.Sum(t => t.Quanitity),
                    TaxSlab = new
                    {
                        x.Product.TaxSlab.Sgst,
                        x.Product.TaxSlab.Cgst
                    }
                }).ToList()
            });
        }

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStockItemsById(int id)
        {
            var result = await _stockItemsService.GetByIdAsync(id);

            return Ok(result);
        }

        //[HttpPost]
        //[Route("create")]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> CreateStockItems([FromRoute] int productId, [FromBody] ProductStockCreateRequest entity)
        //{
        //    var stock = new StockGroups
        //    {
        //        ProductId = productId,
        //        MaterialTypeId = entity.BrandId,
        //        SizeId = entity.SizeId,
        //        BrandId = entity.BrandId,
        //        NetPrice = entity.NetPrice,
        //        Discount = entity.Discount,
        //        Status = (int)EntityStatus.Active
        //    };

        //    var result = await _stockItemsService.CreateAsync(stock, entity.StockQty, (decimal)entity.BuyPrice);

        //    return Ok(result);
        //}

        [HttpPut]
        [Route("update-price")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateStockItems([FromRoute] int productId, [FromBody] ProductStockUpdateRequest entity)
        {
            var stock = new StockGroups
            {
                StockGroupId = productId, // we are sending StockGroupId in route instead of productid
                NetPrice = entity.NetPrice,
                TaxAmount= entity.TaxAmount,
                Mrp= entity.Mrp,
                Discount = entity.Discount,
                SellPrice= entity.SellPrice,
                Status = (int)EntityStatus.Active
            };
            var result = await _stockItemsService.UpdatePriceAsync(stock);

            return Ok(result);
        }

        [HttpGet]
        [Route("prerequisites")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFilterTypesByCategory([FromRoute] int productId)
        {
            var items = await _categoryService.GetCacheAllAsync();

            if (items == null)
                BadRequest("Invalid request");

            var categoryId = await _productDetails.GetCategoryIdAsync(productId);

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

            return Ok(value: new
            {
                materialTypes = materialTypes?.ToList()?.Distinct().Select(s => new { s.MaterialTypeId, s.Name, Selected = false }).ToList(),
                sizes = productSizes?.ToList()?.Distinct().Select(s => new { s.SizeId, s.Name, Selected = false }).ToList(),
                brands = productBrands?.ToList()?.Distinct()?.Select(s => new { s.BrandId, s.Name, Selected = false }).ToList()
            });
        }
    }
}