using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class CustomerMap : DbEntityMapping<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("Accounts.Customers");

            entity.Property(e => e.Address).HasMaxLength(500);

            entity.Property(e => e.City).HasMaxLength(50);

            entity.Property(e => e.CompanyName).HasMaxLength(50);

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.Email).HasMaxLength(256);

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.Property(e => e.Pan)
                .HasColumnName("PAN")
                .HasMaxLength(50);

            entity.Property(e => e.PhoneNo).HasMaxLength(20);

            entity.Property(e => e.TinNo).HasMaxLength(50);

            entity.Property(e => e.ZipCode).HasMaxLength(20);

            entity.HasOne(d => d.Group)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.GroupId);

            base.Configure(entity);
        }
    }
}
