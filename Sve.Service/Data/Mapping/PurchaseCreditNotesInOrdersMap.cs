namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Purchasing;

    internal class CreditNotesInOrdersMap : DbEntityMapping<CreditNotesInOrders>
    {
        public override void Configure(EntityTypeBuilder<CreditNotesInOrders> entity)
        {
            entity.ToTable("Purchasing.CreditNotesInOrders");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CreditNote)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CreditNoteId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PurchaseOrder)
                .WithMany(p => p.CreditNotesInOrders)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(entity);
        }
    }
}
