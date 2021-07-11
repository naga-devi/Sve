namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Purchasing;
    using Sve.Contract.Models.Purchasing;
    using Sve.Contract.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/purchasing/purchasereturns")]
    [ApiController]
    public class PurchasingPurchaseReturnsController : BaseController
    {
        private readonly IPurchaseReturnsService _purchaseReturnsService;

        public PurchasingPurchaseReturnsController
        (
            IPurchaseReturnsService purchaseReturnsService
        )
        {
            _purchaseReturnsService = purchaseReturnsService;
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllPurchaseReturns([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<PurchaseReturns>() ?? new PurchaseReturns();
            var (totalCount, items) = await _purchaseReturnsService.GetByExpressionAsync(index: queryParameters.PageNumber,
                size: queryParameters.PageSize,
                sortColumn: queryParameters.SortField,
                isDescending: queryParameters.IsDescending(),
                filter: filter);

            return Ok(value: new { totalCount , items });
        }        

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPurchaseReturnsById(int id)
        {
            var result = await _purchaseReturnsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreatePurchaseReturns([FromBody] PurchaseReturnCreateModel request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            request.Header.CreatedBy = CurrentUserName;
            var result = await _purchaseReturnsService.CreateAsync(request);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePurchaseReturns([FromBody] PurchaseReturns entity)
        {
			var result = await _purchaseReturnsService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{ids}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePurchaseReturns(string ids)
        {
            var result = await _purchaseReturnsService.DeleteByIdAsync(ids.ToIntegerArray());

            return Ok(result);
        }
    }
}