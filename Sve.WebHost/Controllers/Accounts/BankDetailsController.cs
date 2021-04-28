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
    [Route("api/accounts/bankdetails")]
    [ApiController]
    public class AccountsBankDetailsController : BaseController
    {
        private readonly IBankDetailsService _bankDetailsService;

        public AccountsBankDetailsController
        (
            IBankDetailsService bankDetailService
        )
        {
            _bankDetailsService = bankDetailService;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bankDetailsService.GetAllAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllBankDetails([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<BankDetail>() ?? new BankDetail();
            var (totalCount, items) = await _bankDetailsService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetBankDetailsById(int id)
        {
            var result = await _bankDetailsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateBankDetails([FromBody] BankDetail entity)
        {
            var result = await _bankDetailsService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBankDetails([FromBody] BankDetail entity)
        {
			var result = await _bankDetailsService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{bankIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBankDetails(string bankIds)
        {
            var result = await _bankDetailsService.DeleteByIdAsync(bankIds.ToIntegerArray());

            return Ok(result);
        }
    }
}