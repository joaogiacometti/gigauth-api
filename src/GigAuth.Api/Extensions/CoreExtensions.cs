using GigAuth.Api.Auth;
using GigAuth.Api.Endpoints;
using GigAuth.Api.Middlewares;
using GigAuth.Application;
using GigAuth.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace GigAuth.Api.Extensions;

public static class CoreExtensions
{
    public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplication();
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
    }
    public static void ConfigureMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<CultureMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();
    }

    public static void ConfigureEndpoints(this WebApplication app)
    {
        app.AddUserEndpoints();
        app.AddAuthEndpoints();
    }
}