using GigAuth.Application.UseCases.Users.Create;
using GigAuth.Application.UseCases.Users.Delete;
using GigAuth.Application.UseCases.Users.Get;
using GigAuth.Application.UseCases.Users.Update;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GigAuth.Api.Endpoints;

public static class UserEndpoints
{
    public static void AddUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/user")
            .WithTags("User");

        group.MapPost("/create",
                async ([FromServices] ICreateUserUseCase useCase, [FromBody] RequestCreateUser request) =>
                {
                    await useCase.Execute(request);

                    return Results.Created();
                })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        // TODO: Implement get all with filters
        // TODO: Implement auth
        
        // TODO: Implement auth
        group.MapGet("/get/{id:guid}",
                async ([FromServices] IGetUserUseCase useCase, [FromRoute] Guid id) =>
                Results.Ok(await useCase.Execute(id)))
            .Produces<ResponseUserShort>()
            .Produces(StatusCodes.Status404NotFound);

        // TODO: Implement auth
        group.MapPut("/update/{id:guid}",
                async ([FromServices] IUpdateUserUseCase useCase, [FromBody] RequestUpdateUser request,
                    [FromRoute] Guid id) =>
                {
                    await useCase.Execute(id, request);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        // TODO: Implement auth
        group.MapDelete("/delete/{id:guid}", async ([FromServices] IDeleteUserUseCase useCase, [FromRoute] Guid id) =>
            {
                await useCase.Execute(id);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}