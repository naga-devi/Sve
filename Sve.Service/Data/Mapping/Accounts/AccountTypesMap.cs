using JxNet.Extensions.EFCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sve.Service.Domain.Accounts;

namespace Sve.Service.Data.Mapping
{
    internal class AccountTypesMap : DbEntityMapping<AccountType>
    {
        public override void Configure(EntityTypeBuilder<AccountType> entity)
        {
            entity.HasKey(e => e.AccountTypeId);

            entity.ToTable("Accounts.AccountTypes");

            entity.Property(e => e.Name).HasMaxLength(50);

            base.Configure(entity);
        }
    }
}
