namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Sales;

    internal class SalesSalesOrderDetailMap : DbEntityMapping<SalesOrderDetails>
    {
        public override void Configure(EntityTypeBuilder<SalesOrderDetails> entity)
        {
            entity.ToTable("Sales_OrderDetails");

            entity.Property(e => e.OrderQty).HasColumnType("smallint");

            entity.Property(e => e.CgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.IgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.SgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(19, 4)");


            entity.HasOne(d => d.SalesOrder)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.SalesOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StockGroup)
                .WithMany(p => p.SalesOrderDetails)
                .HasForeignKey(d => d.StockGroupId);

            entity.HasOne(d => d.UnitMeasure)
                    .WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.UnitMeasureId);

            base.Configure(entity);
        }
    }
}
