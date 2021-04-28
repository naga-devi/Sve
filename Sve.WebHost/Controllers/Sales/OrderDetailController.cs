namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Sales;
    using Sve.Contract.Models.Sales;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/sales/{salesOrderId:int}/details")]
    [ApiController]
    public class SalesSalesOrderDetailController : BaseController
    {
        private readonly IOrderDetailService _salesOrderDetailService;

        public SalesSalesOrderDetailController
        (
            IOrderDetailService salesOrderDetailService
        )
        {
            _salesOrderDetailService = salesOrderDetailService;
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllSalesOrderDetail([FromRoute] int salesOrderId, [FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<SalesOrderDetails>();
            var (totalCount, items) = await _salesOrderDetailService.GetByExpressionAsync(salesOrderId: salesOrderId, index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new
            {
                totalCount,
                items = items?.Select(x => new
                {
                    x.Id,
                    x.LineTotal,
                    x.OrderQty,
                    x.StockGroupId,
                    x.UnitPrice,
                    x.CgstAmount,
                    x.SgstAmount,
                    x.StockGroup?.Product?.Name
                })?.ToList()
            });
        }

        [HttpGet]
        [Route("view")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByOrderId([FromRoute] int salesOrderId)
        {
            var items = await _salesOrderDetailService.GetByOrderId(salesOrderId);

            return Ok(value: items?.Select(x => new
            {
                x.ProductId,
                ProductName = x.Product.Name,
                CategoryName = x.Product.Category.Name,
                MaterialTypeName = x.MaterialType.Name,
                SizeName = x.Size.Name,
                BrandName = x.Brand.Name,
                GradeName = x.Grade.Name,
                ColorName = x.Color.Name,
                Sales = x.SalesOrderDetails.Select(p => new
                {
                    p.OrderQty,
                    p.UnitPrice,
                    p.CgstAmount,
                    p.SgstAmount,
                    p.Status,
                    SubTotal = Math.Round(p.LineTotal.Value, 2)
                }).FirstOrDefault()
            })?.ToList());
        }
    }
}