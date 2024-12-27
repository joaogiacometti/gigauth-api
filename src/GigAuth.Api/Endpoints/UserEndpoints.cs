using GigAuth.Api.Extensions;
using GigAuth.Application.UseCases.Users.Delete;
using GigAuth.Application.UseCases.Users.Get;
using GigAuth.Application.UseCases.Users.GetFiltered;
using GigAuth.Application.UseCases.Users.Update;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GigAuth.Api.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/user")
            .WithTags("User")
            .RequireAuthorization(policy => policy.RequirePermission("user"));

        group.MapPost("/get-filtered",
                async ([FromServices] IGetFilteredUsersUseCase useCase, [FromBody] RequestUserFilter filter) =>
                {
                    var result = await useCase.Execute(filter);

                    return result is null ? Results.NoContent() : Results.Ok(result);
                })
            .Produces<List<ResponseUserShort>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapGet("/get/{id:guid}",
                async ([FromServices] IGetUserUseCase useCase, [FromRoute] Guid id) =>
                Results.Ok(await useCase.Execute(id)))
            .Produces<ResponseUserShort>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/update/{id:guid}",
                async ([FromServices] IUpdateUserUseCase useCase, [FromBody] RequestUpdateUser request,
                    [FromRoute] Guid id) =>
                {
                    await useCase.Execute(id, request);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/delete/{id:guid}", async ([FromServices] IDeleteUserUseCase useCase, [FromRoute] Guid id) =>
            {
                await useCase.Execute(id);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }
}