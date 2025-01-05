using GigAuth.Application.UseCases.Auth.Register;
using GigAuth.Application.UseCases.Permissions.Create;
using GigAuth.Application.UseCases.Permissions.Delete;
using GigAuth.Application.UseCases.Permissions.Get;
using GigAuth.Application.UseCases.Permissions.GetFiltered;
using GigAuth.Application.UseCases.Permissions.Update;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Constants;
using GigAuth.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GigAuth.Api.Endpoints;

public static class PermissionEndpoints
{
    public static void AddPermissionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/permission")
            .WithTags("Permission")
            .RequireRateLimiting(RateLimiterConstants.Authorized)
            .RequireAuthorization(policy =>
                policy.RequireRole(RoleConstants.PermissionPermissionName, RoleConstants.AdminPermissionName));

        group.MapPost("/create",
                async ([FromServices] ICreatePermissionUseCase useCase, [FromBody] RequestPermission request) =>
                {
                    await useCase.Execute(request);

                    return Results.Created();
                })
            .Produces(StatusCodes.Status201Created)
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces<ResponseError>(StatusCodes.Status401Unauthorized)
            .Produces<ResponseError>(StatusCodes.Status403Forbidden);

        group.MapPost("/get-filtered",
                async ([FromServices] IGetFilteredPermissionsUseCase useCase, [FromBody] RequestPermissionFilter filter) =>
                {
                    var result = await useCase.Execute(filter);

                    return result is null ? Results.NoContent() : Results.Ok(result);
                })
            .Produces<List<ResponsePermission>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces<ResponseError>(StatusCodes.Status401Unauthorized)
            .Produces<ResponseError>(StatusCodes.Status403Forbidden);

        group.MapGet("/get/{id:guid}",
                async ([FromServices] IGetPermissionUseCase useCase, [FromRoute] Guid id) =>
                Results.Ok(await useCase.Execute(id)))
            .Produces<ResponsePermission>()
            .Produces<ResponseError>(StatusCodes.Status401Unauthorized)
            .Produces<ResponseError>(StatusCodes.Status403Forbidden)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);

        group.MapPut("/update/{id:guid}",
                async ([FromServices] IUpdatePermissionUseCase useCase, [FromBody] RequestPermission request,
                    [FromRoute] Guid id) =>
                {
                    await useCase.Execute(id, request);

                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ResponseError>(StatusCodes.Status400BadRequest)
            .Produces<ResponseError>(StatusCodes.Status401Unauthorized)
            .Produces<ResponseError>(StatusCodes.Status403Forbidden)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);

        group.MapDelete("/delete/{id:guid}", async ([FromServices] IDeletePermissionUseCase useCase, [FromRoute] Guid id) =>
            {
                await useCase.Execute(id);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ResponseError>(StatusCodes.Status401Unauthorized)
            .Produces<ResponseError>(StatusCodes.Status403Forbidden)
            .Produces<ResponseError>(StatusCodes.Status404NotFound);
    }
}