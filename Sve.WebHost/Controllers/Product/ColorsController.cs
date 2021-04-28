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
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/product/colors")]
    [ApiController]
    public class ProductColorsController : BaseController
    {
        private readonly IColorsService _colorsService;

        public ProductColorsController
        (
            IColorsService colorsService
        )
        {
            _colorsService = colorsService;
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllColors([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<Colors>();
            var (totalCount, items) = await _colorsService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new { totalCount , items });
        }        

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetColorsById(int id)
        {
            var result = await _colorsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateColors([FromBody] Colors entity)
        {
            var result = await _colorsService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateColors([FromBody] Colors entity)
        {
			var result = await _colorsService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{colorIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteColors(string colorIds)
        {
            var result = await _colorsService.DeleteByIdAsync(colorIds.ToIntegerArray());

            return Ok(result);
        }
    }
}