using JxNet.Core;
using JxNet.Core.Extensions;
using JxNet.Extensions.ApiBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sve.Contract.Interface.Purchasing;
using Sve.Contract.Models.Purchasing;
using System.Threading.Tasks;

namespace Sve.WebHost.Controllers
{
    [Authorize]
    [Route("api/purchasing/creditnotes")]
    [ApiController]
    public class PurchasingCreditNotesController : BaseController
    {
        private readonly ICreditNotesService _creditNotesService;

        public PurchasingCreditNotesController
        (
            ICreditNotesService creditNotesService
        )
        {
            _creditNotesService = creditNotesService;
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllCreditNotes([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter?.ToEntity<CreditNotes>() ?? new CreditNotes();
            var (totalCount, items) = await _creditNotesService.GetByExpressionAsync(index: queryParameters.PageNumber,
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
        public async Task<IActionResult> GetCreditNotesById(int id)
        {
            var result = await _creditNotesService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateCreditNotes([FromBody] CreditNotes entity)
        {
            var result = await _creditNotesService.CreateAsync(entity);

            return Ok(result);
        }

        [HttpPost]
        [Route("save-with-purchaseorder/{purchaseOrderId:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateCreditNotes(int purchaseOrderId, [FromBody] CreditNotes entity)
        {
            var result = await _creditNotesService.CreateAsync(purchaseOrderId, entity);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCreditNotes([FromBody] CreditNotes entity)
        {
            var result = await _creditNotesService.UpdateAsync(entity);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{ids}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCreditNotes(string ids)
        {
            var result = await _creditNotesService.DeleteByIdAsync(ids.ToIntegerArray());

            return Ok(result);
        }
    }
}