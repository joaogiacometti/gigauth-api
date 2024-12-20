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

        group.MapPost("/login", async ([FromServices] ILoginUseCase useCase, [FromBody] RequestLogin request) => await useCase.Execute(request))
        .Produces<ResponseToken>()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status400BadRequest);
    }
}