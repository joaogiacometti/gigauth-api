using Microsoft.AspNetCore.Authorization;

namespace GigAuth.Api.Auth;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissionsClaims = context.User.Claims
            .Where(c => c.Type == "permissions")
            .Select(c => c.Value)
            .ToList();

        if (permissionsClaims.Count == 0)
            return Task.CompletedTask;

        if (permissionsClaims.Contains(requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}