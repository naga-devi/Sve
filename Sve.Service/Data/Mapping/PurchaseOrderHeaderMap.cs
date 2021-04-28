namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Purchasing;

    internal class PurchaseOrderHeaderMap : DbEntityMapping<PurchaseOrderHeader>
    {
        public override void Configure(EntityTypeBuilder<PurchaseOrderHeader> entity)
        {
            entity.HasKey(e => e.PurchaseOrderId);

            entity.ToTable("Purchasing_PurchaseOrderHeader");

            entity.Property(e => e.CgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Freight).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.GrandTotal).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.IgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.InvoiceNo)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.Property(e => e.NetAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.Property(e => e.RoundOffAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.SgstAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.SubTotal).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.OrderHeader)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(entity);
        }
    }

    internal class PurchasingShipmentsMap : DbEntityMapping<Shipments>
    {
        public override void Configure(EntityTypeBuilder<Shipments> entity)
        {
            entity.HasKey(e => e.ShipmentId);

            entity.ToTable("Purchasing_Shipments");

            entity.Property(e => e.Lrnumber)
                .HasColumnName("LRNumber")
                .HasMaxLength(20);

            entity.Property(e => e.MethodName).HasMaxLength(50);

            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Admin')");

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.Property(e => e.VehicleNumber).HasMaxLength(20);

            base.Configure(entity);
        }
    }

    internal class PurchasingVendorsMap : DbEntityMapping<Vendors>
    {
        public override void Configure(EntityTypeBuilder<Vendors> entity)
        {
            entity.HasKey(e => e.VendorId);

            entity.ToTable("Purchasing_Vendors");

            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Cstno)
                .HasColumnName("CSTNo")
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Pan)
                .HasColumnName("PAN")
                .HasMaxLength(50);

            entity.Property(e => e.PhoneNo)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.Property(e => e.TinNo)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ZipCode)
                .HasMaxLength(20);

            base.Configure(entity);
        }
    }
}
