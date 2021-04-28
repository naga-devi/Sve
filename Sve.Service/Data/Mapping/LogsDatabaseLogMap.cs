namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain;

    internal class LogsDatabaseLogMap : DbEntityMapping<LogsDatabaseLog>
    {
        public override void Configure(EntityTypeBuilder<LogsDatabaseLog> entity)
        {
            entity.HasKey(e => e.DatabaseLogId);

            entity.ToTable("Logs_DatabaseLog");

            entity.Property(e => e.DatabaseLogId);

            entity.Property(e => e.DatabaseUser)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.Event)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.Object)
                .HasMaxLength(128);

            entity.Property(e => e.PostTime)
                .HasColumnType("datetime");

            entity.Property(e => e.Schema)
                .HasMaxLength(128);

            entity.Property(e => e.Tsql)
                .IsRequired()
                .HasColumnName("TSQL");

            entity.Property(e => e.XmlEvent)
                .IsRequired()
                .HasColumnType("xml");

            base.Configure(entity);
        }
    }
}
