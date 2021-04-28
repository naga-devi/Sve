namespace JxNet.Extensions.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    //using JxNet.Extensions.OneSignal;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Product;
    using Sve.Contract.Models.Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/product-category")]
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategoryService;
        //private readonly INotificationService _notificationService;

        public ProductCategoryController(IProductCategoryService productCategoryService/*, INotificationService notificationService*/)
        {
            _productCategoryService = productCategoryService;
            //_notificationService = notificationService;
        }

        //[HttpGet("send-notification")]
        //public async Task<ActionResult> SendNotification()
        //{
        //    Notification myNotification = new Notification
        //    {
        //        contents = "HEELO FROM PUSH NOTIFICATION!",
        //        small_icon = "icon.png",
        //        included_segments = new List<string> { "All" }
        //        //include_player_ids = new List<string> { "OneSignal_playerid_fordevice", "Another_onesignal_playerid" }
        //    };
        //    await _notificationService.SendNotificationToAllAsync("Welcome to JensamTechnogies!");

        //    return Ok();
        //}

        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            var category = new List<ProductCategory>();
            var result = await _productCategoryService.GetCacheAllAsync();

            if (!result.Any(x => x.Name.Equals("All Categories")))
            {
                category = result?.ToList();
                category.Insert(0, new ProductCategory { CategoryId = 0, HasSubCategory = false, Name = "All Categories", ParentId = 0 });
            }

            return Ok(category?.Select(x => new
            {
                id = x.CategoryId,
                name = x.Name,
                hasSubCategory = result.Any(m => x.CategoryId > 0 && m.ParentId == x.CategoryId),
                parentId = x.ParentId
            }).ToList());
        }

        [Authorize]
        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllProductCategory([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<ProductCategory>();

            //if (!filter.Name.IsNullOrEmpty()) {
            //    expColl.AddExpression(x => x.Name.Contains(filter.Name));
            //}

            //Expression<Func<ProductCategory, ProductCategory>> selectExpression = x => new ProductCategory{};
            //IQueryable<ProductCategory> includes(IQueryable<ProductCategory> y) => y.Include(x => x.Subjects).AsQueryable();

            var (totalCount, items) = await _productCategoryService.GetByExpressionAsync(index: queryParameters.PageNumber,
                size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new
            {
                totalCount,
                items = items?.Select(x => new
                {
                    id = x.CategoryId,
                    x.Name,
                    hasSubCategory = items.Any(m => x.CategoryId > 0 && m.ParentId == x.CategoryId),
                    x.ParentId
                }).ToList()
            });
        }

        [Authorize]
        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductCategoryById(int id)
        {
            var result = await _productCategoryService.GetByIdAsync(id);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateProductCategory([FromBody] ProductCategory entity)
        {
            var result = await _productCategoryService.CreateAsync(entity);

            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProductCategory([FromBody] ProductCategory entity)
        {
            var result = await _productCategoryService.UpdateAsync(entity);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{categoryIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProductCategory(string categoryIds)
        {
            var result = await _productCategoryService.DeleteByIdAsync(categoryIds.ToNullableIntegerArray());

            return Ok(result);
        }
    }
}
