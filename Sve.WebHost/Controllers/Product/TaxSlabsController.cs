namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Product;
    using Sve.Contract.Models.Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/product/taxslabs")]
    [ApiController]
    public class ProductTaxSlabsController : BaseController
    {
        private readonly ITaxSlabsService _taxSlabsService;

        public ProductTaxSlabsController
        (
            ITaxSlabsService taxSlabsService
        )
        {
            _taxSlabsService = taxSlabsService;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _taxSlabsService.GetAllAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllTaxSlabs([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<TaxSlabs>();
            var (totalCount, items) = await _taxSlabsService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new { totalCount , items });
        }        

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTaxSlabsById(int id)
        {
            var result = await _taxSlabsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateTaxSlabs([FromBody] TaxSlabs entity)
        {
            var result = await _taxSlabsService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTaxSlabs([FromBody] TaxSlabs entity)
        {
			var result = await _taxSlabsService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{taxSlabIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTaxSlabs(string taxSlabIds)
        {
            var result = await _taxSlabsService.DeleteByIdAsync(taxSlabIds.ToIntegerArray());

            return Ok(result);
        }
    }
}