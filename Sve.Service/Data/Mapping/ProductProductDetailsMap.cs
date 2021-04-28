namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Product;

    internal class ProductProductDetailsMap : DbEntityMapping<ProductDetails>
    {
        public override void Configure(EntityTypeBuilder<ProductDetails> entity)
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("Product_ProductDetails");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Description).HasMaxLength(4000);

            entity.Property(e => e.Hsn).HasColumnName("HSN");

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.CategoryId);

            entity.HasOne(d => d.TaxSlab)
                .WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.TaxSlabId);

            base.Configure(entity);
        }
    }
}
