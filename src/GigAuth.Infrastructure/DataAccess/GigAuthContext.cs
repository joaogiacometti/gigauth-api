using GigAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess;

public class GigAuthContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}