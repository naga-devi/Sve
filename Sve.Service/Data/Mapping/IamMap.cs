namespace Sve.Service.Data.Mapping
{
    using JxNet.Extensions.EFCore.SqlServer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Sve.Service.Domain;
    using Sve.Service.Domain.Iam;

    internal class IamPermissionsMap : DbEntityMapping<Permissions>
    {
        public override void Configure(EntityTypeBuilder<Permissions> entity)
        {
            entity.HasKey(e => e.PermissionId);

            entity.ToTable("Iam_Permissions");

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Name).HasMaxLength(256);

            entity.Property(e => e.Title).HasMaxLength(256);

            base.Configure(entity);
        }
    }

    internal class IamRoleMap : DbEntityMapping<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.HasKey(e => e.RoleId)
                    .HasName("PK_Role");

            entity.ToTable("Iam_Role");

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.IsCoreRole).HasDefaultValueSql("((0))");

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(256);

            base.Configure(entity);
        }
    }

    internal class IamRolePermissionsMap : DbEntityMapping<RolePermissions>
    {
        public override void Configure(EntityTypeBuilder<RolePermissions> entity)
        {
            entity.HasKey(e => e.RolePermissionId);

            entity.ToTable("Iam_RolePermissions");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Permission)
                .WithMany(p => p.IamRolePermissions)
                .HasForeignKey(d => d.PermissionId);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.IamRolePermissions)
                .HasForeignKey(d => d.RoleId);

            base.Configure(entity);
        }
    }

    internal class IamUserRolesMap : DbEntityMapping<UserRoles>
    {
        public override void Configure(EntityTypeBuilder<UserRoles> entity)
        {
            entity.HasKey(e => e.UserRolesId);

            entity.ToTable("Iam_UserRoles");

            entity.Property(e => e.CreatedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.IamUserRoles)
                .HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User)
                .WithMany(p => p.IamUserRoles)
                .HasForeignKey(d => d.UserId);

            base.Configure(entity);
        }
    }

    internal class IamUsersMap : DbEntityMapping<Users>
    {
        public override void Configure(EntityTypeBuilder<Users> entity)
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Iam_Users");

            entity.Property(e => e.Contactno).HasMaxLength(10);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.EmailId).HasMaxLength(200);

            entity.Property(e => e.FullName).HasMaxLength(200);

            entity.Property(e => e.Password).HasMaxLength(200);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(56);

            base.Configure(entity);
        }
    }
}
