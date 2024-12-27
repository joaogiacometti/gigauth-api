using GigAuth.Api.Auth;
using Microsoft.AspNetCore.Authorization;

namespace GigAuth.Api.Extensions;

public static class AuthorizationPolicyBuilderExtensions
{
    public static void RequirePermission(this AuthorizationPolicyBuilder builder, string permission) =>
        builder.AddRequirements(new PermissionRequirement(permission));
}