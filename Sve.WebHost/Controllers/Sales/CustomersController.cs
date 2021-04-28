namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Sales;
    using Sve.Contract.Models.Sales;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/sales/customers")]
    [ApiController]
    public class SalesCustomersController : BaseController
    {
        private readonly ICustomersService _customersService;

        public SalesCustomersController
        (
            ICustomersService customersService
        )
        {
            _customersService = customersService;
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllCustomers([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<Customers>();
            var (totalCount, items) = await _customersService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new { totalCount, items });
        }

        [HttpGet]
        [Route("id/{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCustomersById(int id)
        {
            var result = await _customersService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateCustomers([FromBody] Customers entity)
        {
            var result = await _customersService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCustomers([FromBody] Customers entity)
        {
            var result = await _customersService.UpdateAsync(entity);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{customerIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCustomers(string customerIds)
        {
            var result = await _customersService.DeleteByIdAsync(customerIds.ToIntegerArray());

            return Ok(result);
        }

        [HttpGet]
        [Route("phone-number/{phone?}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByPhoneNo(string phone )
        {
            var result = await _customersService.GetByPhoneNoAsync(phone);

            return Ok(result);
        }

        [HttpGet]
        [Route("tin-no/{tinno?}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByTinNo(string tinno)
        {
            var result = await _customersService.GetByTinNoAsync(tinno);

            return Ok(result);
        }

        //[HttpPost]
        //[Route("create")]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> CreateSalesOrderDetail([FromBody] SalesOrderDetails entity)
        //{
        //    var result = await _salesOrderDetailService.CreateAsync(entity);

        //    return Ok(result);
        //}

        //[HttpPut]
        //[Route("update")]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> UpdateSalesOrderDetail([FromBody] SalesOrderDetails entity)
        //{
        //    var result = await _salesOrderDetailService.UpdateAsync(entity);

        //    return Ok(result);
        //}

        //[HttpDelete]
        //[Route("delete/{ids}")]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> DeleteSalesOrderDetail(string ids)
        //{
        //    var result = await _salesOrderDetailService.DeleteByIdAsync(ids.ToIntegerArray());

        //    return Ok(result);
        //}
    }
}