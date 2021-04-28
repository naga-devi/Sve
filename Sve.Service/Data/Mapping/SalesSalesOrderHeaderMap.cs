namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain.Sales;

    internal class SalesSalesOrderHeaderMap : DbEntityMapping<SalesOrderHeader>
    {
        public override void Configure(EntityTypeBuilder<SalesOrderHeader> entity)
        {
            entity.HasKey(e => e.SalesOrderId);

            entity.ToTable("Sales_OrderHeader");

            entity.Property(e => e.BalanceAmount)
                .HasColumnType("decimal(20, 4)")
                .HasComputedColumnSql("(isnull([GrandTotal]-[PaidAmount],(0)))");

            entity.Property(e => e.Comment).HasMaxLength(500);

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Freight).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.GrandTotal).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.NetAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PaidAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.RoundOffAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(19, 4)");

            entity.Property(e => e.TransactionNo).HasMaxLength(50);

            base.Configure(entity);
        }
    }

    internal class SalesCustomersMap : DbEntityMapping<Customers>
    {
        public override void Configure(EntityTypeBuilder<Customers> entity)
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("Sales_Customers");

            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(500);

            entity.Property(e => e.City).HasMaxLength(50);

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Cstno)
                .HasColumnName("CSTNo")
                .HasMaxLength(50);

            entity.Property(e => e.Email).HasMaxLength(256);

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Pan)
                .HasColumnName("PAN")
                .HasMaxLength(50);

            entity.Property(e => e.PhoneNo).HasMaxLength(20);

            entity.Property(e => e.TinNo).HasMaxLength(50);

            entity.Property(e => e.ZipCode).HasMaxLength(20);


            base.Configure(entity);
        }
    }

    internal class CustomersInOrdersMap : DbEntityMapping<CustomersInOrders>
    {
        public override void Configure(EntityTypeBuilder<CustomersInOrders> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Sales_CustomersInOrders");

            entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomersInOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SalesOrder)
                .WithMany(p => p.CustomersInOrders)
                .HasForeignKey(d => d.SalesOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(entity);
        }
    }
}
