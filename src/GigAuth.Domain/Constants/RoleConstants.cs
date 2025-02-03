namespace GigAuth.Domain.Constants;

public static class RoleConstants
{
    public const string AdminRoleName = "Admin";
    public const string ManagerRoleName = "Manager";
    public const string UserRoleName = "User";

    public const string UserPermissionName = "User";
    public const string RolePermissionName = "Role";
    public const string PermissionPermissionName = "Permission";
    public const string AdminPermissionName = "Admin";

    public static readonly DateTime SeedDate = new(2025, 1, 1, 12, 12, 59, DateTimeKind.Utc);

    public static readonly Guid AdminRoleId = new("728e5486-ddd3-42cd-b8c5-3278181b1d36");
    public static readonly Guid ManagerRoleId = new("9eaeca53-2cfc-409c-a411-63bf7f69f8c6");
    public static readonly Guid UserRoleId = new("f66caaf2-f359-4aee-a057-784023736d67");

    public static readonly Guid UserPermissionId = new("ba3b36b0-68c6-4bc7-84fd-2fac867ea86c");
    public static readonly Guid RolePermissionId = new("f55923e4-bcc8-4397-a9e3-2f9ff0bd025e");
    public static readonly Guid PermissionPermissionId = new("6aa94319-e9ee-4d79-8cad-c6c3cb69520c");
    public static readonly Guid AdminPermissionId = new("f574d33c-d8bf-4dec-9173-09b6580f25ab");

    public static readonly Guid UserRolePermissionId = new("97773159-aa53-4761-8c27-d87705dd9280");
    public static readonly Guid RoleRolePermissionId = new("3f4760f9-f709-41b1-a07c-1d4b914f53f3");
    public static readonly Guid PermissionRolePermissionId = new("caab3f43-fdba-4568-8b92-27b8383ee64e");
    public static readonly Guid AdminRolePermissionId = new("f9210a4e-fdaf-4cb2-a1b0-18925b493d6a");
}