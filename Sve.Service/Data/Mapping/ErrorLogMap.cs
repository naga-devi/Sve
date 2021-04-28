namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain;

    internal class ErrorLogMap : DbEntityMapping<LogsErrorLog>
    {
        public override void Configure(EntityTypeBuilder<LogsErrorLog> entity)
        {
            entity.HasKey(e => e.ErrorLogId);

            entity.ToTable("Logs_ErrorLog");

            entity.Property(e => e.ErrorLogId);

            entity.Property(e => e.ErrorLine);

            entity.Property(e => e.ErrorMessage)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(e => e.ErrorNumber);

            entity.Property(e => e.ErrorProcedure)
                .HasMaxLength(126);

            entity.Property(e => e.ErrorSeverity);

            entity.Property(e => e.ErrorState);

            entity.Property(e => e.ErrorTime)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(128);

            base.Configure(entity);
        }
    }
}
