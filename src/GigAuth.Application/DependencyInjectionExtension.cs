using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Application.UseCases.Users.Create;
using Microsoft.Extensions.DependencyInjection;

namespace GigAuth.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
    }
}