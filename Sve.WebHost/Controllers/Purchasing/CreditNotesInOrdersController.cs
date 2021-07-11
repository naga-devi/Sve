namespace Sve.WebHost.Controllers
{
	using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Purchasing;
    using Sve.Contract.Models.Purchasing;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/purchasing/creditnotesinorders")]
    [ApiController]
    public class PurchasingCreditNotesInOrdersController : BaseController
    {
        private readonly ICreditNotesInOrdersService _creditNotesInOrdersService;

        public PurchasingCreditNotesInOrdersController
        (
            ICreditNotesInOrdersService creditNotesInOrdersService
        )
        {
            _creditNotesInOrdersService = creditNotesInOrdersService;
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllCreditNotesInOrders([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<CreditNotesInOrders>() ?? new CreditNotesInOrders();
            var (totalCount, items) = await _creditNotesInOrdersService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetCreditNotesInOrdersById(int id)
        {
            var result = await _creditNotesInOrdersService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateCreditNotesInOrders([FromBody] CreditNotesInOrders entity)
        {
            var result = await _creditNotesInOrdersService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCreditNotesInOrders([FromBody] CreditNotesInOrders entity)
        {
			var result = await _creditNotesInOrdersService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{ids}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCreditNotesInOrders(string ids)
        {
            var result = await _creditNotesInOrdersService.DeleteByIdAsync(ids.ToIntegerArray());

            return Ok(result);
        }
    }
}