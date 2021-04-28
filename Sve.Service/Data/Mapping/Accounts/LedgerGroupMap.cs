using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class LedgerGroupMap : DbEntityMapping<LedgerGroup>
    {
        public override void Configure(EntityTypeBuilder<LedgerGroup> entity)
        {
            entity.HasKey(e => e.GroupId);

            entity.ToTable("Accounts.LedgerGroups");

            entity.Property(e => e.Name).HasMaxLength(50);

            base.Configure(entity);
        }
    }
}
