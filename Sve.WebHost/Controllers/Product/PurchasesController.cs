namespace Sve.WebHost.Controllers
{
    using JxNet.Extensions.WebHost.Models;
    using JxNet.Platform.Extension.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Product;
    using Sve.Contract.Models.Product;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/product/{StockGroupId:int}/purchases")]
    [ApiController]
    public class PurchasesController : BaseController
    {
        private readonly IPurchasesService _stockItemsHistoryService;

        public PurchasesController
        (
            IPurchasesService stockItemsHistoryService
        )
        {
            _stockItemsHistoryService = stockItemsHistoryService;
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllStockItemsHistory([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<Purchases>();

            var (totalCount, items) = await _stockItemsHistoryService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new { totalCount, items });
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateStockItemsHistory([FromRoute] int StockGroupId, [FromBody] PurchaseCreateRequest entity)
        {
            var purchase = new Purchases
            {
                StockGroupId = StockGroupId,
                BuyPrice = entity.BuyPrice,
                StockedQty = entity.StockedQty,
                Status = (int)EntityStatus.Active
            };
            var result = await _stockItemsHistoryService.CreateAsync(purchase);

            return Ok(result);
        }
    }
}