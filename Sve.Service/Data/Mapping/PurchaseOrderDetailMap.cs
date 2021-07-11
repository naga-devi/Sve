namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Purchasing;

    internal class PurchaseOrderDetailMap : DbEntityMapping<PurchaseOrderDetail>
    {
        public override void Configure(EntityTypeBuilder<PurchaseOrderDetail> entity)
        {
            entity.ToTable("Purchasing_PurchaseOrderDetail");

            entity.Property(e => e.CgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Discount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.IgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Mrp).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.RejectedQty).HasColumnType("decimal(8, 2)");

            entity.Property(e => e.SgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.Property(e => e.StockedQty)
                .HasColumnType("decimal(9, 2)")
                .HasComputedColumnSql("(isnull([ReceivedQty]-[RejectedQty],(0.00)))");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.PurchaseOrder)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StockGroup)
                .WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(d => d.StockGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.UnitMeasure)
                    .WithMany(p => p.PurchaseOrderDetail)
                    .HasForeignKey(d => d.UnitMeasureId);

            base.Configure(entity);
        }
    }
}
