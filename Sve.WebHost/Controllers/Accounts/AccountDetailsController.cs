namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Accounts;
    using Sve.Contract.Models.Accounts;
    using System.Linq;
    using System.Threading.Tasks;
    using Enums = Sve.Contract.Enums;

    [Authorize]
    [Route("api/accounts/accountdetails")]
    [ApiController]
    public class AccountsAccountDetailsController : BaseController
    {
        private readonly IAccountDetailsService _accountDetailsService;

        public AccountsAccountDetailsController
        (
            IAccountDetailsService accountDetailsService
        )
        {
            _accountDetailsService = accountDetailsService;
        }

        [HttpGet]
        [Route("all/{vocherTypeId:int}/{customerId:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByCustomerId(int vocherTypeId, int customerId)
        {
            var result = await _accountDetailsService.GetByCustomerId(customerId);

            var companyAccounts = result?.Where(x => x.Customer.IsParentCompany == true)?.Select(x => new
            {
                x.AccountId,
                Name = $"{x.AccountNo}-{x.Bank?.BankName}({x.Bank?.IFSC})"
            }).ToList() ;

            var otherAccounts = result?.Where(x => x.Customer.IsParentCompany == false)?.Select(x => new
            {
                x.AccountId,
                Name = $"{x.AccountNo}-{x.Bank?.BankName}({x.Bank?.IFSC})"
            }).ToList();

            var returnModel = vocherTypeId == (int)Enums.VoucherType.Payment ? new { fromAccounts = companyAccounts, toAccounts = otherAccounts } : new { fromAccounts = otherAccounts, toAccounts = companyAccounts };

            return Ok(returnModel);
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllCustomerAccountDetails([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<AccountDetail>() ?? new AccountDetail();
            var (totalCount, items) = await _accountDetailsService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetCustomerAccountDetailsById(int id)
        {
            var result = await _accountDetailsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateCustomerAccountDetails([FromBody] AccountDetail entity)
        {
            var result = await _accountDetailsService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCustomerAccountDetails([FromBody] AccountDetail entity)
        {
            var result = await _accountDetailsService.UpdateAsync(entity);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{accountIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCustomerAccountDetails(string accountIds)
        {
            var result = await _accountDetailsService.DeleteByIdAsync(accountIds.ToIntegerArray());

            return Ok(result);
        }
    }
}