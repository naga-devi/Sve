namespace Sve.WebHost.Controllers
{
    using JxNet.Extensions.WebHost.Models.Reports;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Sales;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using JxNet.Core.Extensions;

    [Authorize]
    [Route("api/sales/reports")]
    [ApiController]
    public class SalesReportController : BaseController
    {
        private readonly IReportService _reportService;

        public SalesReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("day-ledger/{date?}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByPhoneNo(string date)
        {
            var today = DateTime.Now;

            if (!string.IsNullOrEmpty(date))
            {
                DateTime.TryParse(date, out today);
            }

            var result = await _reportService.GetDayLedger(today);

            if (result.HasItems())
            {
                var returnModel = result.GroupBy(x => new { x.PaymodeText })
                    .Select(x => new DayLedgerViewModel
                    {
                        PaymodeText = x.Key.PaymodeText,
                        TotalQuantity = x.Sum(t => t.TotalQuantity),
                        TotalAmount = x.Sum(t => t.TotalAmount),
                        DiscountAmount = x.Sum(t => t.DiscountAmount),
                        NetAmount = x.Sum(t => t.NetAmount),
                        RoundOffAmount = x.Sum(t => t.RoundOffAmount),
                        GrandTotal = x.Sum(t => t.GrandTotal),
                        PaidAmount = x.Sum(t => t.PaidAmount),
                        BalanceAmount = x.Sum(t => t.BalanceAmount),
                    })
                    .ToList();

                returnModel.Add(new DayLedgerViewModel
                {
                    PaymodeText = "Total",
                    TotalQuantity = result.Sum(t => t.TotalQuantity),
                    TotalAmount = result.Sum(t => t.TotalAmount),
                    DiscountAmount = result.Sum(t => t.DiscountAmount),
                    NetAmount = result.Sum(t => t.NetAmount),
                    RoundOffAmount = result.Sum(t => t.RoundOffAmount),
                    GrandTotal = result.Sum(t => t.GrandTotal),
                    PaidAmount = result.Sum(t => t.PaidAmount),
                    BalanceAmount = result.Sum(t => t.BalanceAmount),
                });

                return Ok(returnModel);
            }

            return Ok(result);
        }

        //private List<ChartItemsViewModel> PrepareChartData(List<DayLedgerViewModel> dayLedgers)
        //{
        //    var data = new List<ChartItemsViewModel>();

        //    var item = new ChartItemsViewModel {
        //        Name= "Quantity",
        //        Series = new List<ChartSeriesViewModel>
        //        {

        //        }
        //    };


        //    return null;
        //}
    }
}