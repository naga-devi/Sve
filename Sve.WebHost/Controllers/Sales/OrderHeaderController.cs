namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Core.Helpers;
    using JxNet.Extensions.ApiBase;
    using JxNet.Extensions.DinkPDF;
    using JxNet.Extensions.WebHost.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Sales;
    using Sve.Contract.Models.Sales;
    using Sve.Contract.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/sales/orders")]
    [ApiController]
    public class SalesSalesOrderHeaderController : BaseController
    {
        private readonly IOrderHeaderService _salesOrderHeaderService;
        private readonly IPDFService _pdfService;
        private readonly AppSettings _appSettings;

        public SalesSalesOrderHeaderController
        (
            AppSettings appSettings,
            IOrderHeaderService salesOrderHeaderService,
            IPDFService pdfService
        )
        {
            _salesOrderHeaderService = salesOrderHeaderService;
            _pdfService = pdfService;
            _appSettings = appSettings;
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllSalesOrderHeader([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<SalesOrderHeader>();
            var (totalCount, items) = await _salesOrderHeaderService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new
            {
                totalCount,
                items = items?.Select(x => new
                {
                    x.SalesOrderId,
                    x.OrderDate,
                    //OrderDate = x.OrderDate.ToDDMMYYY(),
                    x.Status,
                    x.StatusText,
                    x.GrandTotal,
                    x.TotalQuantity,
                    x.TotalAmount,
                    x.DiscountPercentage,
                    x.NetAmount,
                    x.Freight,
                    x.RoundOffAmount,
                    x.PaidAmount,
                    x.BalanceAmount,
                    x.Paymode,
                    x.PaymodeText,
                    x.TransactionNo,
                    x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn.ToDDMMYYY_HHMMTT(),
                    x.CustomersInOrders?.FirstOrDefault()?.Customer
                }
                ).ToList()
            });
        }

        //      [HttpGet]
        //      [Route("id/{id:int}")]
        //      [ProducesResponseType(400)]
        //      [ProducesResponseType(404)]
        //      public async Task<IActionResult> GetSalesOrderHeaderById(int id)
        //      {
        //          var result = await _salesOrderHeaderService.GetByIdAsync(id);

        //          return Ok(result);
        //      }

        [HttpPost]
        [Route("confirm/{salesOrderId:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ConfirmOrder(int salesOrderId, [FromBody] OrderConfirmRequest orderConfirmRequest)
        {
            if (salesOrderId == 0 || orderConfirmRequest == null)
            {
                return Ok(new ResponseResult((int)Status.Error, Status.Error, "Invalid request.", null));
            }

            var result = await _salesOrderHeaderService.ConfirmOrder(salesOrderId, orderConfirmRequest);

            return Ok(result);
        }

        [HttpGet]
        [Route("download-invoice/{salesOrderId:int}")]
        public async Task<IActionResult> DownloadInvoice(int salesOrderId)
        {
            var result = await _salesOrderHeaderService.GetInvoiceByIdAsync(salesOrderId);
            var replaceParams = GetInvoiceValues(result);
            var htmlContent = FileHelper.BuildHtml(Path.Combine(_appSettings.WebRootPath, "templates/invoice.html"), replaceParams);
            var (isSuccess, message, fileContent) = await _pdfService.CreateAsync(htmlContent);
            return File(fileContent, JxNet.Extensions.ApiBase.GlobalConstants.ContentType.PDF, $"{salesOrderId}-invoice.pdf");
        }

        private Dictionary<string, string> GetInvoiceValues(InvoiceModel model)
        {

            var sb = new StringBuilder();

            if (model.InvoiceItems.HasItems())
            {
                int index = 1;
                model.InvoiceItems.ForEach(x =>
                {
                    sb.Append(GetRow(index, "", "", "", x.OrderQty, x.UnitPrice, x.LineTotal));
                    index++;
                });
            }

            model.TotalAmountBeforeTax = model?.InvoiceItems?.Sum(x => x.LineTotal);

            var invoice = new Dictionary<string, string>
            {
                {"{{ConsumerName}}", model?.Customer?.CosumerName },
                {"{{InvoiceNo}}", model?.InvoiceNo.ToString() },
                {"{{Address}}", model?.Customer?.Address },
                {"{{InvoiceDate}}", model?.InvoiceDate.ToDDMMYYY()},
                {"{{TransportName}}", model?.TransportName },
                {"{{GSTIN}}", model?.Customer?.GSTIN },
                {"{{VehicleNo}}", model?.VehicleNo},
                {"{{CellNo}}", model?.Customer?.CellNo },
                {"{{PlaceOfSupply}}", model?.PlaceOfSupply },
                {"{{Rupeesinwords}}", ((double)model?.TotalAmountAfterTax).ConvertAmount() },
                {"{{TotalAmountBeforeTax}}", model?.InvoiceItems?.Sum(x=> x.LineTotal).ToRoundString() },
                {"{{AddCGST}}", model?.InvoiceItems?.Sum(x=> x.Cgst * x.OrderQty)?.ToRoundString() },
                {"{{AddSGST}}",model?.InvoiceItems?.Sum(x=> x.Sgst * x.OrderQty)?.ToRoundString() },
                {"{{AddTransport}}", model?.AddTransport?.ToString() },
                {"{{Discount}}", model?.Discount?.ToRoundString() },
                {"{{TotalAmountAfterTax}}", model?.TotalAmountAfterTax?.ToRoundString() },
                { "{{producItemsRow}}", sb?.ToString()}
            };

            return invoice;
        }

        private string GetRow(int sNo, string hsnCode, string size, string productDescription, int qty, decimal rate, decimal amount)
        {

            var itemRow = $"<tr><td class='tg-0pky'>{sNo}</td><td class='tg-0pky'>{hsnCode}</td><td class='tg-0pky'>{size}</td><td class='tg-0pky'>{productDescription}</td><td class='tg-0pky'>{qty}</td><td class='tg-0pky'>{rate}</td><td class='tg-0pky'>{amount}</td></tr>";

            return itemRow;
        }

        //      [HttpPut]
        //      [Route("update")]
        //      [ProducesResponseType(400)]
        //      [ProducesResponseType(404)]
        //      public async Task<IActionResult> UpdateSalesOrderHeader([FromBody] SalesOrderHeader entity)
        //      {
        //	var result = await _salesOrderHeaderService.UpdateAsync(entity);

        //          return Ok(result);
        //      }

        //[HttpDelete]
        //      [Route("delete/{salesOrderIds}")]
        //      [ProducesResponseType(400)]
        //      [ProducesResponseType(404)]
        //      public async Task<IActionResult> DeleteSalesOrderHeader(string salesOrderIds)
        //      {
        //          var result = await _salesOrderHeaderService.DeleteByIdAsync(salesOrderIds.ToIntegerArray());

        //          return Ok(result);
        //      }
    }
}