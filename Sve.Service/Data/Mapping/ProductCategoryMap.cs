namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Product;

    internal class ProductCategoryMap : DbEntityMapping<ProductCategory>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> entity)
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("Product_ProductCategory");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.HasSubCategory).HasDefaultValueSql("((0))");

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            //entity.HasData(
            //    new ProductCategory
            //    {
            //        CategoryId = 1,
            //        Name = "Plumbling",

            //    },
            //    new ProductCategory
            //    {
            //        CategoryId = 2,
            //        Name = "Tiles",
            //    }
            //);

            base.Configure(entity);
        }
    }

    internal class ProductBrandsMap : DbEntityMapping<ProductBrands>
    {
        public override void Configure(EntityTypeBuilder<ProductBrands> entity)
        {
            entity.HasKey(e => e.BrandId);

            entity.ToTable("Product_Brands");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.ProductBrands)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Brands_Product_ProductCategory");

            base.Configure(entity);
        }
    }

    internal class ProductSizesMap : DbEntityMapping<ProductSizes>
    {
        public override void Configure(EntityTypeBuilder<ProductSizes> entity)
        {
            entity.HasKey(e => e.SizeId);

            entity.ToTable("Product_Sizes");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Sizes_Product_Sizes");
            base.Configure(entity);
        }
    }

    internal class MaterialTypesMap : DbEntityMapping<ProductMaterialTypes>
    {
        public override void Configure(EntityTypeBuilder<ProductMaterialTypes> entity)
        {
            entity.HasKey(e => e.MaterialTypeId);

            entity.ToTable("Product_MaterialTypes");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.ProductMaterialType)
                .HasForeignKey(d => d.CategoryId);

            base.Configure(entity);
        }
    }

    internal class TaxSlabsMap : DbEntityMapping<ProductTaxSlabs>
    {
        public override void Configure(EntityTypeBuilder<ProductTaxSlabs> entity)
        {
            entity.HasKey(e => e.TaxSlabId);

            entity.ToTable("Product_TaxSlabs");

            entity.Property(e => e.Cgst)
                .HasColumnType("decimal(21, 6)")
                .HasComputedColumnSql("([TotalTax]/(2))");

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.Property(e => e.Sgst)
                .HasColumnType("decimal(21, 6)")
                .HasComputedColumnSql("([TotalTax]/(2))");

            entity.Property(e => e.TotalTax).HasColumnType("decimal(19, 4)");

            base.Configure(entity);
        }
    }

    internal class ProductGradesMap : DbEntityMapping<Grades>
    {
        public override void Configure(EntityTypeBuilder<Grades> entity)
        {
            entity.HasKey(e => e.GradeId);

            entity.ToTable("Product_Grades");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.ProductGrades)
                .HasForeignKey(d => d.CategoryId);

            base.Configure(entity);
        }
    }

    internal class ProductColorsMap : DbEntityMapping<Colors>
    {
        public override void Configure(EntityTypeBuilder<Colors> entity)
        {
            entity.HasKey(e => e.ColorId);

            entity.ToTable("Product_Colors");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.ProductColors)
                .HasForeignKey(d => d.CategoryId);

            base.Configure(entity);
        }
    }
}
