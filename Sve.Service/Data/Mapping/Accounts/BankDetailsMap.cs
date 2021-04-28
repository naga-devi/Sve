using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class BankDetailsMap : DbEntityMapping<BankDetail>
    {
        public override void Configure(EntityTypeBuilder<BankDetail> entity)
        {
            entity.HasKey(e => e.BankId);

            entity.ToTable("Accounts.BankDetails");

            entity.Property(e => e.Address).HasMaxLength(250);

            entity.Property(e => e.BankName).HasMaxLength(20);

            entity.Property(e => e.BranchName).HasMaxLength(20);

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.DdpayableAddress)
                .HasColumnName("DDPayableAddress")
                .HasMaxLength(250);

            entity.Property(e => e.Ifsc)
                .HasColumnName("IFSC")
                .HasMaxLength(10);

            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.Property(e => e.Phone).HasMaxLength(20);

            base.Configure(entity);
        }
    }
}
