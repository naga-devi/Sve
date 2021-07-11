namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Purchasing;

    internal class PurchaseReturnsMap : DbEntityMapping<PurchaseReturns>
    {
        public override void Configure(EntityTypeBuilder<PurchaseReturns> entity)
        {
            entity.ToTable("Purchasing_PurchaseReturns");

            entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.GrandTotal).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.Property(e => e.ReturnDate).HasColumnType("date");

            entity.Property(e => e.RoundOff).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.PurchaseOrder)
                .WithMany(p => p.PurchaseReturns)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(entity);
        }
    }
}
