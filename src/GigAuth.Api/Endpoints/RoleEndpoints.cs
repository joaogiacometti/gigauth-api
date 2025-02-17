using GigAuth.Application.UseCases.Roles.Create;
using GigAuth.Application.UseCases.Roles.Delete;
using GigAuth.Application.UseCases.Roles.Get;
using GigAuth.Application.UseCases.Roles.GetFiltered;
using GigAuth.Application.UseCases.Roles.Update;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Constants;
using GigAuth.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GigAuth.Api.Endpoints;

public static class RoleEndpoints
{
    public static void AddRoleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/role")
            .WithTags("Role")
            .RequireRateLimiting(RateLimiterConstants.Authorized)
            .RequireAuthorization(policy =>
                policy.RequireRole(RoleConstants.RolePermissionName, RoleConstants.AdminPermissionName));

        group.MapPost("/create",
                async ([FromServices] ICreateRoleUseCase useCase, [FromBody] RequestRole request) =>
                {
                    await useCase.Execute(request);

                    return Results.Created();
                })
            .Produces(StatusCodes.Status201Created)
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapPost("/get-filtered",
                async ([FromServices] IGetFilteredRolesUseCase useCase, [FromBody] RequestRoleFilter filter) =>
                {
                    var result = await useCase.Execute(filter);

                    return result is null ? Results.NoContent() : Results.Ok(result);
                })
            .Produces<List<ResponseRole>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        group.MapGet("/get/{id:guid}",
                async ([FromServices] IGetRoleUseCase useCase, [FromRoute] Guid id) =>
                Results.Ok(await useCase.Execute(id)))
            .Produces<ResponseRole>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);

        group.MapPut("/update/{id:guid}",
                async ([FromServices] IUpdateRoleUseCase useCase, [FromBody] RequestRole request,
                    [FromRoute] Guid id) =>
                {
                    await useCase.Execute(id, request);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);

        group.MapDelete("/delete/{id:guid}", async ([FromServices] IDeleteRoleUseCase useCase, [FromRoute] Guid id) =>
            {
                await useCase.Execute(id);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);
    }
}