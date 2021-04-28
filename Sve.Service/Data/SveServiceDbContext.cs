namespace Sve.Service.Data
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    internal class SveServiceDbContext : DbContextBase, ISveServiceDbContext
    {
        public SveServiceDbContext(DbContextOptions<SveServiceDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
            ////delete your database prior to making
            //Database.EnsureCreated();
            //RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)Database.GetService<IDatabaseCreator>();
            //databaseCreator.CreateTables();
            //ChangeTracker.AutoDetectChangesEnabled = true;
            //ChangeTracker.LazyLoadingEnabled = false;
            Database.SetCommandTimeout(120);
            //Database.Migrate();
        }
    }
}
