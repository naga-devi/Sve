namespace Sve.WebHost.Controllers
{
    using JxNet.Core;
    using JxNet.Core.Extensions;
    using JxNet.Extensions.ApiBase;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Sve.Contract.Interface.Purchasing;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/purchasing/{purchaseOrderId:int}/details")]
    [ApiController]
    public class PurchasingPurchaseOrderDetailController : BaseController
    {
        private readonly IOrderDetailService _purchaseOrderDetailService;

        public PurchasingPurchaseOrderDetailController
        (
            IOrderDetailService purchaseOrderDetailService
        )
        {
            _purchaseOrderDetailService = purchaseOrderDetailService;
        }

        [HttpGet]
        [Route("view")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByOrderId([FromRoute] int purchaseOrderId)
        {
            var items = await _purchaseOrderDetailService.GetByOrderId(purchaseOrderId);

            return Ok(value: items?.Select(x => new
            {
                x.ProductId,
                ProductName = x.Product.Name,
                CategoryName = x.Product.Category.Name,
                MaterialTypeName = x.MaterialType.Name,
                SizeName = x.Size.Name,
                BrandName = x.Brand.Name,
                GradeName = x.Grade.Name,
                ColorName = x.Color.Name,
                Purchase = x.PurchaseOrderDetails.Select(p => new
                {
                    p.Quanitity,
                    p.UnitPrice,
                    p.Discount,
                    p.UnitMeasureId,
                    UnitMeasure =p.UnitMeasure.Name,
                    p.CgstAmount,
                    p.SgstAmount,
                    p.IgstAmount,
                    p.Mrp,
                    SubTotal = Math.Round((decimal)(p.Quanitity * (p.UnitPrice + p.CgstAmount + p.SgstAmount)), 2)
                }).FirstOrDefault()
            })?.ToList());
        }
    }
}