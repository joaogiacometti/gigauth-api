using GigAuth.Domain.Constants;
using GigAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.Seed;

public static class RoleSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = RoleConstants.AdminRoleId, Name = RoleConstants.AdminRoleName,
                CreatedDate = RoleConstants.SeedDate, UpdatedDate = RoleConstants.SeedDate
            },
            new Role
            {
                Id = RoleConstants.ManagerRoleId, Name = RoleConstants.ManagerRoleName,
                CreatedDate = RoleConstants.SeedDate, UpdatedDate = RoleConstants.SeedDate
            },
            new Role
            {
                Id = RoleConstants.UserRoleId, Name = RoleConstants.UserRoleName, CreatedDate = RoleConstants.SeedDate,
                UpdatedDate = RoleConstants.SeedDate
            }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission
            {
                Id = RoleConstants.UserPermissionId, Name = RoleConstants.UserPermissionName,
                CreatedDate = RoleConstants.SeedDate, UpdatedDate = RoleConstants.SeedDate
            },
            new Permission
            {
                Id = RoleConstants.RolePermissionId, Name = RoleConstants.RolePermissionName,
                CreatedDate = RoleConstants.SeedDate, UpdatedDate = RoleConstants.SeedDate
            },
            new Permission
            {
                Id = RoleConstants.AdminPermissionId, Name = RoleConstants.AdminPermissionName,
                CreatedDate = RoleConstants.SeedDate, UpdatedDate = RoleConstants.SeedDate
            }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission
            {
                Id = RoleConstants.UserRolePermissionId, RoleId = RoleConstants.ManagerRoleId,
                PermissionId = RoleConstants.UserPermissionId
            },
            new RolePermission
            {
                Id = RoleConstants.RoleRolePermissionId, RoleId = RoleConstants.ManagerRoleId,
                PermissionId = RoleConstants.RolePermissionId
            },
            new RolePermission
            {
                Id = RoleConstants.AdminRolePermissionId, RoleId = RoleConstants.AdminRoleId,
                PermissionId = RoleConstants.AdminPermissionId
            }
        );
    }
}