namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Product;

    internal class ProductStockGroupMap : DbEntityMapping<StockGroups>
    {
        public override void Configure(EntityTypeBuilder<StockGroups> entity)
        {
            entity.HasKey(e => e.StockGroupId);

            entity.ToTable("Product_StockGroups");

            entity.Property(e => e.BasicMrp)
                .HasColumnName("BasicMRP")
                .HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Cgst).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Description).HasMaxLength(4000);

            entity.Property(e => e.Discount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.MaterialTypeId);//.HasComment("UPVC/CPVC");

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.Property(e => e.Mrp)
                .HasColumnName("MRP")
                .HasColumnType("decimal(19, 4)");

            entity.Property(e => e.NetPrice).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.SellPrice).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Sgst).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.Property(e => e.TaxAmount).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Brand)
                .WithMany(p => p.StockGroups)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Color)
                .WithMany(p => p.StockGroups)
                .HasForeignKey(d => d.ColorId);

            entity.HasOne(d => d.Grade)
                .WithMany(p => p.StockGroups)
                .HasForeignKey(d => d.GradeId);

            entity.HasOne(d => d.MaterialType)
                .WithMany(p => p.StockGroups)
                .HasForeignKey(d => d.MaterialTypeId);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.StockGroups)
                .HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.Size)
                .WithMany(p => p.StockGroups)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(entity);
        }
    }    
}