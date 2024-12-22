using GigAuth.Application.UseCases.Auth.ForgotPassword;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Application.UseCases.Users.Create;
using GigAuth.Application.UseCases.Users.Delete;
using GigAuth.Application.UseCases.Users.Update;
using Microsoft.Extensions.DependencyInjection;

namespace GigAuth.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
    }
}