using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class AccountDetailMap : DbEntityMapping<AccountDetail>
    {
        public override void Configure(EntityTypeBuilder<AccountDetail> entity)
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("Accounts.CustomerAccountDetails");

            entity.Property(e => e.AccountNo).HasMaxLength(50);

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Bank)
                .WithMany(p => p.AccountDetails)
                .HasForeignKey(d => d.BankId);

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.AccountDetails)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            base.Configure(entity);
        }
    }
}
