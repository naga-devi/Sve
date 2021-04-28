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
    [Route("api/accounts/ledgergroups")]
    [ApiController]
    public class AccountsLedgerGroupsController : BaseController
    {
        private readonly ILedgerGroupsService _ledgerGroupsService;

        public AccountsLedgerGroupsController
        (
            ILedgerGroupsService ledgerGroupsService
        )
        {
            _ledgerGroupsService = ledgerGroupsService;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ledgerGroupsService.GetAllAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllLedgerGroups([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<LedgerGroup>() ?? new LedgerGroup();
            var (totalCount, items) = await _ledgerGroupsService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetLedgerGroupsById(int id)
        {
            var result = await _ledgerGroupsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateLedgerGroups([FromBody] LedgerGroup entity)
        {
            var result = await _ledgerGroupsService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateLedgerGroups([FromBody] LedgerGroup entity)
        {
			var result = await _ledgerGroupsService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{groupIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteLedgerGroups(string groupIds)
        {
            var result = await _ledgerGroupsService.DeleteByIdAsync(groupIds.ToIntegerArray());

            return Ok(result);
        }
    }
}