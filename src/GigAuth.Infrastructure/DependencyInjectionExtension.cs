using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.ForgotPasswordTokens;
using GigAuth.Domain.Repositories.Permissions;
using GigAuth.Domain.Repositories.RefreshTokens;
using GigAuth.Domain.Repositories.Roles;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Services.Security.Cryptography;
using GigAuth.Domain.Services.Security.Tokens;
using GigAuth.Domain.Services.SupabaseProvider;
using GigAuth.Infrastructure.DataAccess;
using GigAuth.Infrastructure.DataAccess.Repositories;
using GigAuth.Infrastructure.Extensions;
using GigAuth.Infrastructure.Services.Security.Cryptography;
using GigAuth.Infrastructure.Services.Security.Tokens;
using GigAuth.Infrastructure.Services.SupabaseProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GigAuth.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.IsTestEnvironment())
        {
            AddDbContext(services, configuration);
            services.AddHealthChecks()
                .AddDbContextCheck<GigAuthContext>();
        }

        AddRepositories(services);
        AddServices(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();

        services.AddScoped<IRoleReadOnlyRepository, RoleRepository>();
        services.AddScoped<IRoleWriteOnlyRepository, RoleRepository>();

        services.AddScoped<IPermissionReadOnlyRepository, PermissionRepository>();
        services.AddScoped<IPermissionWriteOnlyRepository, PermissionRepository>();

        services.AddScoped<IForgotPasswordTokenWriteOnlyRepository, ForgotPasswordTokenRepository>();
        services.AddScoped<IForgotPasswordTokenReadOnlyRepository, ForgotPasswordTokenRepository>();

        services.AddScoped<IRefreshTokenWriteOnlyRepository, RefreshTokenRepository>();
        services.AddScoped<IRefreshTokenReadOnlyRepository, RefreshTokenRepository>();
    }
    
    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ICryptography, Cryptography>();
        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddScoped<ISupabaseClientFactory, SupabaseClientFactory>();
        services.AddScoped<IStorageService, StorageService>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection") ??
            configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<GigAuthContext>(options => { options.UseNpgsql(connectionString); });
    }
}