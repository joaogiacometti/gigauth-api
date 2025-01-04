using GigAuth.Application.UseCases.Auth.ChangePassword;
using GigAuth.Application.UseCases.Auth.ForgotPassword;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Application.UseCases.Auth.RefreshToken;
using GigAuth.Application.UseCases.Auth.Register;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GigAuth.Api.Endpoints;

public static class AuthEndpoints
{
    public static void AddAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Auth")
            .RequireRateLimiting("Global");

        group.MapPost("/register",
                async ([FromServices] IRegisterUseCase useCase, [FromBody] RequestRegister request) =>
                {
                    await useCase.Execute(request);

                    return Results.Created();
                })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/refresh-token",
                async ([FromServices] IRefreshTokenUseCase useCase, [FromBody] RequestRefreshToken request) =>
                Results.Ok(await useCase.Execute(request)))
            .Produces<ResponseToken>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);
        
        group.MapPost("/login",
                async ([FromServices] ILoginUseCase useCase, [FromBody] RequestLogin request) =>
                await useCase.Execute(request))
            .Produces<ResponseToken>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/forgot-password/{username}",
                async ([FromServices] IForgotPasswordUseCase useCase, [FromRoute] string userName) =>
                {
                    await useCase.Execute(userName);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/change-password",
                async ([FromServices] IChangePasswordUseCase useCase, [FromBody] RequestChangePassword request) =>
                {
                    await useCase.Execute(request);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}