using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class PayModeMap : DbEntityMapping<PayMode>
    {
        public override void Configure(EntityTypeBuilder<PayMode> entity)
        {
            entity.HasKey(e => e.PayModeId);

            entity.ToTable("Accounts.PayModes");

            entity.Property(e => e.Name).HasMaxLength(30);

            base.Configure(entity);
        }
    }
}
