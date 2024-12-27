using Microsoft.AspNetCore.Authorization;

namespace GigAuth.Api.Auth;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
