using GigAuth.Application.UseCases.Roles.GetFiltered;
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
            .RequireAuthorization(policy => policy.RequireRole(RoleConstants.RolePermissionName, RoleConstants.AdminPermissionName));
        
        group.MapPost("/get-filtered",
                async ([FromServices] IGetFilteredRolesUseCase useCase, [FromBody] RequestRoleFilter filter) =>
                {
                    var result = await useCase.Execute(filter);

                    return result is null ? Results.NoContent() : Results.Ok(result);
                })
            .Produces<List<ResponseRole>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
    }
}