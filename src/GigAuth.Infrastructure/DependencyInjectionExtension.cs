using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Cryptography;
using GigAuth.Domain.Security.Tokens;
using GigAuth.Infrastructure.DataAccess;
using GigAuth.Infrastructure.DataAccess.Repositories;
using GigAuth.Infrastructure.Extensions;
using GigAuth.Infrastructure.Security.Cryptography;
using GigAuth.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GigAuth.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.IsTestEnvironment())
            AddDbContext(services, configuration);

        services.AddScoped<ICryptography, Cryptography>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<GigAuthContext>(options =>
        {
            options.UseNpgsql(connectionString); 
        });
    }
}