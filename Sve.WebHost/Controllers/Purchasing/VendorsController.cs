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
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/purchasing/vendors")]
    [ApiController]
    public class PurchasingVendorsController : BaseController
    {
        private readonly IVendorsService _vendorsService;

        public PurchasingVendorsController
        (
            IVendorsService vendorsService
        )
        {
            _vendorsService = vendorsService;
        }

        [HttpGet]
        [Route("phone-number/{phone?}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByPhoneNo(string phone)
        {
            var result = await _vendorsService.GetByPhoneNoAsync(phone);

            return Ok(result);
        }

        [HttpGet]
        [Route("tin-no/{tinno?}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByTinNo(string tinno)
        {
            var result = await _vendorsService.GetByTinNoAsync(tinno);

            return Ok(result);
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllVendors([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<Vendors>();
            var (totalCount, items) = await _vendorsService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new { totalCount, items });
        }

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetVendorsById(int id)
        {
            var result = await _vendorsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateVendors([FromBody] Vendors entity)
        {
            var result = await _vendorsService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateVendors([FromBody] Vendors entity)
        {
            var result = await _vendorsService.UpdateAsync(entity);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{vendorIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteVendors(string vendorIds)
        {
            var result = await _vendorsService.DeleteByIdAsync(vendorIds.ToIntegerArray());

            return Ok(result);
        }
    }
}