namespace Sve.Service.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DbInitializer : IDbInitializer
    {
        //private readonly ISveServiceDbContext _dbContext;

        public DbInitializer(/*ISveServiceDbContext cdrDbContext*/)
        {
            //_dbContext = cdrDbContext;
        }

        public void Migrate()
        {
           //_dbContext.Database.Migrate();
        }

        public void Seed()
        {

            //if (!_dbContext.UserRoles.Any())
            //{
            //    var salt = new byte[128 / 8];
            //    using (var rng = RandomNumberGenerator.Create())
            //    {
            //        rng.GetBytes(salt);
            //    }

            //    var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            //        password: "QAZwsx123",
            //        salt: salt,
            //        prf: KeyDerivationPrf.HMACSHA512,
            //        iterationCount: 10000,
            //        numBytesRequested: 256 / 8));

            //    _dbContext.UserRoles.AddRange(new List<UserRoleModel>
            //    {
            //        new UserRoleModel
            //        {
            //            RoleInfo = new RoleModel()
            //            {
            //                RoleName = "Admin"
            //            },
            //            UserInfo = new UserModel()
            //            {
            //                FirstName = "autoSeedFirstName",
            //                LastName = "autoSeedLastName",
            //                Username = "Admin",
            //                Salt = salt,
            //                Password = hashedPassword
            //            }
            //        }
            //    });

            //}

            //_dbContext.SaveChanges();
        }
    }
}
