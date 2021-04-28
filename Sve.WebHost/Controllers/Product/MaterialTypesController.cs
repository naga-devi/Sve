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
    [Route("api/product/materialtypes")]
    [ApiController]
    public class ProductMaterialTypesController : BaseController
    {
        private readonly IMaterialTypesService _materialTypesService;

        public ProductMaterialTypesController
        (
            IMaterialTypesService materialTypesService
        )
        {
            _materialTypesService = materialTypesService;
        }

        [HttpPost]
        [Route("find")]
		[ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllMaterialTypes([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<MaterialTypes>();
            var (totalCount, items) = await _materialTypesService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new { totalCount , items });
        }        

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMaterialTypesById(int id)
        {
            var result = await _materialTypesService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateMaterialTypes([FromBody] MaterialTypes entity)
        {
            var result = await _materialTypesService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMaterialTypes([FromBody] MaterialTypes entity)
        {
			var result = await _materialTypesService.UpdateAsync(entity);

            return Ok(result);
        }

		[HttpDelete]
        [Route("delete/{materialTypeIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMaterialTypes(string materialTypeIds)
        {
            var result = await _materialTypesService.DeleteByIdAsync(materialTypeIds.ToNullableIntegerArray());

            return Ok(result);
        }
    }
}