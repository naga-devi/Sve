namespace Sve.Service.Domain.Product
{
    using JxNet.Extensions.EFCore.SqlServer;
    using System;

    internal partial class ProductImages : CreateAuditEntityBase
    {
        public int? ProductId { get; set; }
        public int ImageId { get; set; }
        public string Path { get; set; }
        public int? ImageType { get; set; }
        public int? Status { get; set; }

        public ProductDetails Product { get; set; }
    }
}
