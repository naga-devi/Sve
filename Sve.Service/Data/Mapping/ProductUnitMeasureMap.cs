namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Product;

    internal class ProductUnitMeasureMap : DbEntityMapping<UnitMeasure>
    {
        public override void Configure(EntityTypeBuilder<UnitMeasure> entity)
        {
            entity.HasKey(e => e.UnitMeasureId);

            entity.ToTable("Product_UnitMeasures");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            base.Configure(entity);
        }
    }
}
