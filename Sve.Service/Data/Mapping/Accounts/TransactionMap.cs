using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class TransactionMap : DbEntityMapping<Transaction>
    {
        public override void Configure(EntityTypeBuilder<Transaction> entity)
        {
            entity.HasKey(e => e.TransactionId);

            entity.ToTable("Accounts.Transactions");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.PaidAmount)
                .HasColumnType("decimal(18, 4)")
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.PaidDate).HasColumnType("datetime");

            entity.Property(e => e.Remarks).HasMaxLength(250);

            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.AccountType)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountTypeId);

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.PayMode)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PayModeId);

            entity.HasOne(d => d.VoucherType)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.VoucherTypeId);

            base.Configure(entity);
        }
    }
}
