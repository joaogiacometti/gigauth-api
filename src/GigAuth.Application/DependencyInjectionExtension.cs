using GigAuth.Application.UseCases.Auth.ChangePassword;
using GigAuth.Application.UseCases.Auth.ForgotPassword;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Application.UseCases.Auth.RefreshToken;
using GigAuth.Application.UseCases.Auth.Register;
using GigAuth.Application.UseCases.Roles.Create;
using GigAuth.Application.UseCases.Roles.Delete;
using GigAuth.Application.UseCases.Roles.Get;
using GigAuth.Application.UseCases.Roles.GetFiltered;
using GigAuth.Application.UseCases.Users.Delete;
using GigAuth.Application.UseCases.Users.Get;
using GigAuth.Application.UseCases.Users.GetFiltered;
using GigAuth.Application.UseCases.Users.Update;
using Microsoft.Extensions.DependencyInjection;

namespace GigAuth.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUseCase, RegisterUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        
        services.AddScoped<IGetUserUseCase, GetUserUseCase>();
        services.AddScoped<IGetFilteredUsersUseCase, GetFilteredUsersUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        
        services.AddScoped<ICreateRoleUseCase, CreateRoleUseCase>();
        services.AddScoped<IGetRoleUseCase, GetRoleUseCase>();
        services.AddScoped<IGetFilteredRolesUseCase, GetFilteredRolesUseCase>();
        services.AddScoped<IDeleteRoleUseCase, DeleteRoleUseCase>();
    }
}