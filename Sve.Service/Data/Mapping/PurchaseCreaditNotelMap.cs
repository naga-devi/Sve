namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Purchasing;

    internal class PurchaseCreaditNotelMap : DbEntityMapping<CreditNotes>
    {
        public override void Configure(EntityTypeBuilder<CreditNotes> entity)
        {
            entity.HasKey(e => e.CreditNoteId);

            entity.ToTable("Purchasing.CreditNotes");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Discount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.CreditNotes)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(entity);
        }
    }
}
