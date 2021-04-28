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
    [Route("api/accounts/customers")]
    [ApiController]
    public class AccountsCustomersController : BaseController
    {
        private readonly ICustomersService _customersService;

        public AccountsCustomersController
        (
            ICustomersService customersService
        )
        {
            _customersService = customersService;
        }

        [HttpGet]
        [Route("all/{groupId:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll(int groupId, [FromQuery] string name)
        {
            var result = await _customersService.GetAllAsync(groupId, name);

            return Ok(result?.Select(x => new { x.CustomerId, name = string.IsNullOrEmpty(x.TinNo) ? $"{x.CompanyName}" : $"{x.CompanyName}({x.TinNo})" }).ToList());
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllCustomers([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<Customer>() ?? new Customer();
            var (totalCount, items) = await _customersService.GetByExpressionAsync(index: queryParameters.PageNumber,
                size: queryParameters.PageSize,
                sortColumn: queryParameters.SortField,
                isDescending: queryParameters.IsDescending(),
                filter: filter);

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
        public async Task<IActionResult> CreateCustomers([FromBody] Customer entity)
        {
            var result = await _customersService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCustomers([FromBody] Customer entity)
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
    }
}