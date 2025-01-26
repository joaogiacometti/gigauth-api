using GigAuth.Domain.Entities;
using GigAuth.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess;

public class GigAuthContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<User> Users { get; init; }
    public required DbSet<Role> Roles { get; init; }
    public required DbSet<ForgotPasswordToken> ForgotPasswordTokens { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; init; }
    public required DbSet<Permission> Permissions { get; init; }
    
    public required DbSet<UserRole> UserRoles { get; init; }
    public required DbSet<RolePermission> RolePermissions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GigAuthContext).Assembly);

        RoleSeedData.Seed(modelBuilder);
    }
}