namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Purchasing;
    using Sve.Contract.Models.Purchasing;
    using Sve.Contract.ViewModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/purchasing/purchase-order")]
    [ApiController]
    public class PurchasingPurchaseOrderHeaderController : BaseController
    {
        private readonly IOrderHeaderService _purchaseOrderHeaderService;

        public PurchasingPurchaseOrderHeaderController
        (
            IOrderHeaderService purchaseOrderHeaderService
        )
        {
            _purchaseOrderHeaderService = purchaseOrderHeaderService;
        }

        [HttpPost]
        [Route("find")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> FindAllPurchaseOrderHeader([FromBody] QueryParametersModel queryParameters)
        {
            var filter = queryParameters.Filter.ToEntity<PurchaseOrderHeader>();
            var (totalCount, items) = await _purchaseOrderHeaderService.GetByExpressionAsync(index: queryParameters.PageNumber, size: queryParameters.PageSize, sortColumn: queryParameters.SortField, isDescending: queryParameters.IsDescending(), filter: filter);

            return Ok(value: new
            {
                totalCount,
                items = items?.Select(x => new
                {
                    x.PurchaseOrderId,
                    PurchaseDate = x.PurchaseDate.ToDDMMYYY(),
                    x.InvoiceNo,
                    x.GrandTotal,
                    x.CreatedBy,
                    CreatedOn = x.CreatedOn.ToDDMMYYY(),
                    Quantity = x.PurchaseOrderDetail.Sum(m => m.Quanitity),
                    x.VendorId,
                    x.Vendor.PhoneNo,
                    x.Vendor.CompanyName
                }).ToList()
            });
        }

        [HttpPost]
        [Route("save")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreatePurchaseOrderHeader([FromBody] PurchaseOrderCreateRequest request)
        {
            var result = await _purchaseOrderHeaderService.SaveAsync(request);

            return Ok(result);
        }

        [HttpGet]
        [Route("edit/{purchaseOrderId:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByOrderId(int purchaseOrderId)
        {
            var order = await _purchaseOrderHeaderService.GetById(purchaseOrderId);

            return Ok(new
            {
                order.PurchaseOrderId,
                vendorId = order.VendorId,
                companyName = order.Vendor.CompanyName,
                tinNo = order.Vendor.TinNo,
                email = order.Vendor.Email,
                phoneNo = order.Vendor.PhoneNo,
                address = order.Vendor.Address,
                invoiceNo = order.InvoiceNo,
                purchaseDate = order.PurchaseDate,
                totalAmount = order.TotalAmount,
                discount = order.DiscountAmount,
                netAmount = order.NetAmount,
                cgstAmount = order.CgstAmount,
                sgstAmount = order.SgstAmount,
                subTotal = order.SubTotal,
                roundOffAmount = order.RoundOffAmount,
                grandTotal = order.GrandTotal,
                details = order.PurchaseOrderDetail?.Select(p => new
                {
                    p.Id,
                    p.StockGroup.ProductId,
                    p.StockGroup.Product.CategoryId,
                    p.StockGroup.MaterialTypeId,
                    p.StockGroup.SizeId,
                    p.StockGroup.BrandId,
                    p.StockGroup.ColorId,
                    p.StockGroup.GradeId,
                    p.Quanitity,
                    p.UnitPrice,
                    p.Discount,
                    p.CgstAmount,
                    p.SgstAmount,
                    p.IgstAmount,
                    p.Mrp,
                    p.UnitMeasureId,

                    totalCgstAmount = Math.Round((decimal)(p.Quanitity * p.CgstAmount), 2),
                    totalSgstAmount = Math.Round((decimal)(p.Quanitity * p.SgstAmount), 2),
                    taxableAmount = GetTaxableAmount(p),
                }).ToList()
            });
        }

        [HttpDelete]
        [Route("delete/{purchaseOrderIds}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePurchaseOrder(string purchaseOrderIds)
        {
            var result = await _purchaseOrderHeaderService.DeleteByIdAsync(purchaseOrderIds.ToIntegerArray());

            return Ok(result);
        }

        private static decimal? GetTaxableAmount(PurchaseOrderDetail purchase)
        {
            decimal? subtotal = 0;

            if (purchase.UnitPrice > 0 && purchase.CgstAmount > 0 && purchase.SgstAmount > 0)
            {
                if (purchase.Discount > 0)
                {
                    subtotal =Math.Round((decimal)(purchase.UnitPrice - ((decimal)(purchase.UnitPrice * purchase.Discount) / 100)), 2);
                }
                else
                {
                    subtotal = purchase.UnitPrice;
                }
            }

            return Math.Round((decimal)(subtotal * purchase.Quanitity), 2);
        }
    }
}