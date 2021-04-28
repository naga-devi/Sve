using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class VoucherTypeMap : DbEntityMapping<VoucherType>
    {
        public override void Configure(EntityTypeBuilder<VoucherType> entity)
        {
            entity.HasKey(e => e.VoucherTypeId);

            entity.ToTable("Accounts.VoucherTypes");

            entity.Property(e => e.Name).HasMaxLength(50);

            base.Configure(entity);
        }
    }
}
