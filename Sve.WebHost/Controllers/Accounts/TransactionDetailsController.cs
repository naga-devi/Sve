namespace Sve.WebHost.Controllers
{
	using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Accounts;
    using Sve.Contract.Models.Accounts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/accounts/transactiondetails")]
    [ApiController]
    public class AccountsTransactionDetailsController : BaseController
    {
        private readonly ITransactionDetailsService _transactionDetailsService;

        public AccountsTransactionDetailsController
        (
            ITransactionDetailsService transactionDetailsService
        )
        {
            _transactionDetailsService = transactionDetailsService;
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllTransactionDetails([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<TransactionDetail>() ?? new TransactionDetail();
            var (totalCount, items) = await _transactionDetailsService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetTransactionDetailsById(int id)
        {
            var result = await _transactionDetailsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateTransactionDetails([FromBody] TransactionDetail entity)
        {
            var result = await _transactionDetailsService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTransactionDetails([FromBody] TransactionDetail entity)
        {
			var result = await _transactionDetailsService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{ids}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTransactionDetails(string ids)
        {
            var result = await _transactionDetailsService.DeleteByIdAsync(ids.ToIntegerArray());

            return Ok(result);
        }
    }
}