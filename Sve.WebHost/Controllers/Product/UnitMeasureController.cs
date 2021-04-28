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
    [Route("api/product/unitmeasure")]
    [ApiController]
    public class ProductUnitMeasureController : BaseController
    {
        private readonly IUnitMeasureService _unitMeasureService;

        public ProductUnitMeasureController
        (
            IUnitMeasureService unitMeasureService
        )
        {
            _unitMeasureService = unitMeasureService;
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitMeasureService.GetAllAsync();

            return Ok(result);
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllUnitMeasure([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<UnitMeasure>();
            var (totalCount, items) = await _unitMeasureService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new { totalCount , items });
        }        

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUnitMeasureById(int id)
        {
            var result = await _unitMeasureService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateUnitMeasure([FromBody] UnitMeasure entity)
        {
            var result = await _unitMeasureService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUnitMeasure([FromBody] UnitMeasure entity)
        {
			var result = await _unitMeasureService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{unitMeasureIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUnitMeasure(string unitMeasureIds)
        {
            var result = await _unitMeasureService.DeleteByIdAsync(unitMeasureIds.ToIntegerArray());

            return Ok(result);
        }
    }
}