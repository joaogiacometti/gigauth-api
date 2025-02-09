using CommonTestsUtilities.Entities;
using GigAuth.Domain.Constants;
using GigAuth.Domain.Entities;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Domain.Security.Tokens;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WebApi.Tests.Resources;

namespace WebApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("gigauth")
        .WithUsername("postgres")
        .WithPassword("root")
        .Build();

    public UserIdentityManager Admin { get; private set; } = null!;
    public UserIdentityManager User { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                services.AddDbContext<GigAuthContext>(config =>
                {
                    config.UseNpgsql(_dbContainer.GetConnectionString());
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<GigAuthContext>();
                var cryptography = scope.ServiceProvider.GetRequiredService<ICryptography>();
                var tokenProvider = scope.ServiceProvider.GetRequiredService<ITokenProvider>();
                
                dbContext.Database.Migrate();
                
                Admin = AddUser(dbContext, cryptography, tokenProvider, true);
                User = AddUser(dbContext, cryptography, tokenProvider);
                
                dbContext.SaveChanges();
            });
    }
    
    private static UserIdentityManager AddUser(GigAuthContext dbContext, ICryptography cryptography,
        ITokenProvider tokenProvider, bool isAdmin = false)
    {
        var user = UserBuilder.Build();

        var password = user.PasswordHash;
        user.PasswordHash = cryptography.Encrypt(user.PasswordHash);

        dbContext.Users.Add(user);

        if (isAdmin)
            SetAdminRole(dbContext, user);

        var token = tokenProvider.GenerateToken(user);
        var refreshToken = tokenProvider.GenerateRefreshToken(user.Id);
        
        dbContext.RefreshTokens.Add(refreshToken);

        return new UserIdentityManager(user, password, token, refreshToken.Token);
    }
    
    private static void SetAdminRole(GigAuthContext dbContext, User user)
    {
        var adminRole = dbContext.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Single(r => r.Id.Equals(RoleConstants.AdminRoleId));

        user.UserRoles.Add(new UserRole {Role = adminRole, RoleId = adminRole.Id, UserId = user.Id});
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}