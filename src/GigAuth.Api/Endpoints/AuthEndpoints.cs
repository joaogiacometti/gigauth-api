using GigAuth.Application.UseCases.Auth.ChangePassword;
using GigAuth.Application.UseCases.Auth.ForgotPassword;
using GigAuth.Application.UseCases.Auth.Login;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GigAuth.Api.Endpoints;

public static class AuthEndpoints
{
    public static void AddAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth")
            .WithName("auth")
            .WithTags("auth");

        group.MapPost("/login",
                async ([FromServices] ILoginUseCase useCase, [FromBody] RequestLogin request) =>
                await useCase.Execute(request))
            .WithName("Login")
            .Produces<ResponseToken>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("{username}",
                async ([FromServices] IForgotPasswordUseCase useCase, [FromRoute] string userName) =>
                {
                    await useCase.Execute(userName);

                    return Results.NoContent();
                })
            .WithName("Forgot Password")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("", async ([FromServices] IChangePasswordUseCase useCase, [FromBody] RequestChangePassword request) =>
            {
                await useCase.Execute(request);

                return Results.NoContent();
            })
            .WithName("Change Password")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}