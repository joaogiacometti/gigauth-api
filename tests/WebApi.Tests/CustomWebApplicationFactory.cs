using CommonTestsUtilities.Entities;
using GigAuth.Api;
using GigAuth.Domain.Constants;
using GigAuth.Domain.Entities;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Domain.Security.Tokens;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Tests.Resources;

namespace WebApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public UserIdentityManager Admin { get; private set; } = null!;
    public UserIdentityManager User { get; private set; } = null!;
    public GigAuthContext DbContext { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<GigAuthContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<GigAuthContext>();
                var cryptography = scope.ServiceProvider.GetRequiredService<ICryptography>();
                var tokenProvider = scope.ServiceProvider.GetRequiredService<ITokenProvider>();

                Setup(dbContext, cryptography, tokenProvider);

                dbContext.SaveChanges();
            });
    }

    private void Setup(GigAuthContext dbContext, ICryptography cryptography, ITokenProvider tokenProvider)
    {
        var admin = AddUser(dbContext, cryptography, tokenProvider, isAdmin: true);
        var user = AddUser(dbContext, cryptography, tokenProvider);
        
        Admin = admin;
        User = user;
        DbContext = dbContext;

        dbContext.SaveChanges();
    }
    
    private static UserIdentityManager AddUser(GigAuthContext dbContext, ICryptography cryptography, ITokenProvider tokenProvider, bool isAdmin = false)
    {
        var user = UserBuilder.Build();
        
        var password = user.PasswordHash;
        user.PasswordHash = cryptography.Encrypt(user.PasswordHash);
        
        dbContext.Users.Add(user);

        if(isAdmin)
            AddAdminRole(dbContext, user);
        
        var token = tokenProvider.GenerateToken(user);

        return new UserIdentityManager(user, password, token);
    }

    private static void AddAdminRole(GigAuthContext dbContext, User user)
    {
        var adminPermission = new Permission()
        {
            Id = RoleConstants.AdminPermissionId,
            Name = RoleConstants.AdminPermissionName,
        };
    
        var adminRole = new Role()
        {
            Id = RoleConstants.AdminRoleId,
            Name = RoleConstants.AdminRoleName,
        };

        var rolePermission = new RolePermission()
        {
            Role = adminRole,
            Permission = adminPermission,
            RoleId = RoleConstants.AdminRoleId,
            PermissionId = adminPermission.Id,
        };
    
        var userRole = new UserRole
        {
            User = user,
            Role = adminRole,
            RoleId = adminRole.Id,
            UserId = user.Id,
        };
    
        dbContext.Permissions.Add(adminPermission);
        dbContext.Roles.Add(adminRole);
        dbContext.RolePermissions.Add(rolePermission);
        dbContext.UserRoles.Add(userRole);
    }
}