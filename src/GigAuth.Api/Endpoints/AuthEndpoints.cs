using GigAuth.Application.UseCases.Auth.ChangePassword;
using GigAuth.Application.UseCases.Auth.ForgotPassword;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Application.UseCases.Auth.RefreshToken;
using GigAuth.Application.UseCases.Auth.Register;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GigAuth.Api.Endpoints;

public static class AuthEndpoints
{
    public static void AddAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Auth")
            .RequireRateLimiting(RateLimiterConstants.Global);

        group.MapPost("/register",
                async ([FromServices] IRegisterUseCase useCase, [FromBody] RequestRegister request) =>
                {
                    await useCase.Execute(request);

                    return Results.Created();
                })
            .Produces(StatusCodes.Status201Created)
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces<ResponseError>(StatusCodes.Status409Conflict);

        group.MapPost("/refresh-token",
                async ([FromServices] IRefreshTokenUseCase useCase, [FromBody] RequestRefreshToken request) =>
                Results.Ok(await useCase.Execute(request)))
            .Produces<ResponseToken>()
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces<ResponseError>(StatusCodes.Status401Unauthorized)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);

        group.MapPost("/login",
                async ([FromServices] ILoginUseCase useCase, [FromBody] RequestLogin request) =>
                await useCase.Execute(request))
            .Produces<ResponseToken>()
            .Produces<ResponseError>(StatusCodes.Status401Unauthorized);

        group.MapPost("/forgot-password/{username}",
                async ([FromServices] IForgotPasswordUseCase useCase, [FromRoute] string userName) =>
                {
                    await useCase.Execute(userName);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);

        group.MapPut("/change-password",
                async ([FromServices] IChangePasswordUseCase useCase, [FromBody] RequestChangePassword request) =>
                {
                    await useCase.Execute(request);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);
    }
}