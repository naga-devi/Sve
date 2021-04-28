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
    [Route("api/accounts/accounttypes")]
    [ApiController]
    public class AccountsAccountTypesController : BaseController
    {
        private readonly IAccountTypesService _accountTypesService;

        public AccountsAccountTypesController
        (
            IAccountTypesService AccountTypeService
        )
        {
            _accountTypesService = AccountTypeService;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountTypesService.GetAllAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllAccountTypes([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<AccountType>() ?? new AccountType();
            var (totalCount, items) = await _accountTypesService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetAccountTypesById(int id)
        {
            var result = await _accountTypesService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateAccountTypes([FromBody] AccountType entity)
        {
            var result = await _accountTypesService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAccountTypes([FromBody] AccountType entity)
        {
			var result = await _accountTypesService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{accountTypeIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAccountTypes(string accountTypeIds)
        {
            var result = await _accountTypesService.DeleteByIdAsync(accountTypeIds.ToIntegerArray());

            return Ok(result);
        }
    }
}