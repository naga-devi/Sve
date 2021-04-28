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
    [Route("api/accounts/vouchertypes")]
    [ApiController]
    public class AccountsVoucherTypesController : BaseController
    {
        private readonly IVoucherTypesService _voucherTypesService;

        public AccountsVoucherTypesController
        (
            IVoucherTypesService voucherTypesService
        )
        {
            _voucherTypesService = voucherTypesService;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _voucherTypesService.GetAllAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllVoucherTypes([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<VoucherType>() ?? new VoucherType();
            var (totalCount, items) = await _voucherTypesService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetVoucherTypesById(int id)
        {
            var result = await _voucherTypesService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateVoucherTypes([FromBody] VoucherType entity)
        {
            var result = await _voucherTypesService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateVoucherTypes([FromBody] VoucherType entity)
        {
			var result = await _voucherTypesService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{voucherTypeIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteVoucherTypes(string voucherTypeIds)
        {
            var result = await _voucherTypesService.DeleteByIdAsync(voucherTypeIds.ToIntegerArray());

            return Ok(result);
        }
    }
}