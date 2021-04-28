namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Accounts;
    using Sve.Contract.Models.Accounts;
    using Sve.WebHost.Models.Accounts;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/accounts/transactions")]
    [ApiController]
    public class AccountsTransactionsController : BaseController
    {
        private readonly ITransactionsService _transactionsService;

        public AccountsTransactionsController
        (
            ITransactionsService transactionsService
        )
        {
            _transactionsService = transactionsService;
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllTransactions([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<Transactions>() ?? new Transactions();
            var (totalCount, items) = await _transactionsService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetTransactionsById(int id)
        {
            var result = await _transactionsService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateTransactions([FromBody] TransactionCreateModel request)
        {
            var entity = new Transactions
            {
                AccountTypeId = request.AccountTypeId,
                CustomerId = request.CustomerId,
                PaidAmount = request.PaidAmount,
                PaidDate = request.PaidDate,
                PayModeId = request.PayModeId,
                Remarks = request.Remarks,
                TransactionId = request.TransactionId ?? 0,
                VoucherTypeId = request.VoucherTypeId,
                Status = (int)EntityStatus.Active
            };

            if (request.AccountTypeId == (int)Sve.Contract.Enums.AccountType.BankAccounts)
            {
                var details = new TransactionDetail
                {
                    TransactionId = request.TransactionId,
                    ChequeDate = request.ChequeDate,
                    ChequeNo = request.ChequeNo,
                    FromAccountId = request.FromAccountId,
                    Id = request.Id,
                    ToAccountId = request.ToAccountId,
                    Utrno = request.Utrno
                };
                var res = await _transactionsService.CreateAsync(entity, details);

                return Ok(res);
            }

            var result = await _transactionsService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTransactions([FromBody] Transactions entity)
        {
            var result = await _transactionsService.UpdateAsync(entity);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{transactionIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTransactions(string transactionIds)
        {
            var result = await _transactionsService.DeleteByIdAsync(transactionIds.ToIntegerArray().Select(x => (long)x).ToArray());

            return Ok(result);
        }
    }
}