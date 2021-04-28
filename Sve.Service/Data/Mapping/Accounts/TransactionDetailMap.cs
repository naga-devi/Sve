using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class TransactionDetailMap : DbEntityMapping<TransactionDetail>
    {
        public override void Configure(EntityTypeBuilder<TransactionDetail> entity)
        {
            entity.ToTable("Accounts.TransactionDetails");

            entity.Property(e => e.ChequeDate).HasColumnType("date");

            entity.Property(e => e.ChequeNo).HasMaxLength(30);

            entity.Property(e => e.TransactionDate).HasColumnType("date");

            entity.Property(e => e.Utrno)
                .HasColumnName("UTRNo")
                .HasMaxLength(50);

            entity.HasOne(d => d.FromAccount)
                .WithMany(p => p.FromAccounts)
                .HasForeignKey(d => d.FromAccountId);

            entity.HasOne(d => d.ToAccount)
                .WithMany(p => p.ToAccounts)
                .HasForeignKey(d => d.ToAccountId);

            entity.HasOne(d => d.Transaction)
                .WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.TransactionId);

            base.Configure(entity);
        }
    }
}
