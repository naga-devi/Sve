namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Product;

    internal class ProductImageMap : DbEntityMapping<ProductImages>
    {
        public override void Configure(EntityTypeBuilder<ProductImages> entity)
        {
            entity.HasKey(e => e.ImageId);

            entity.ToTable("Product_ProductImages");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Path)
                .IsRequired()
                .HasMaxLength(250);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId);

            base.Configure(entity);
        }
    }
}
