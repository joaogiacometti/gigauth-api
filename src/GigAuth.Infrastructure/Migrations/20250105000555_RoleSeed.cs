using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GigAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoleSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate" },
                values: new object[] { new Guid("ba3b36b0-68c6-4bc7-84fd-2fac867ea86c"), new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc), null, "User", new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("728e5486-ddd3-42cd-b8c5-3278181b1d36"), new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc), null, "Admin", new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc) },
                    { new Guid("9eaeca53-2cfc-409c-a411-63bf7f69f8c6"), new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc), null, "Manager", new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc) },
                    { new Guid("f66caaf2-f359-4aee-a057-784023736d67"), new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc), null, "User", new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[] { new Guid("97773159-aa53-4761-8c27-d87705dd9280"), new Guid("ba3b36b0-68c6-4bc7-84fd-2fac867ea86c"), new Guid("9eaeca53-2cfc-409c-a411-63bf7f69f8c6") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: new Guid("97773159-aa53-4761-8c27-d87705dd9280"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("728e5486-ddd3-42cd-b8c5-3278181b1d36"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f66caaf2-f359-4aee-a057-784023736d67"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba3b36b0-68c6-4bc7-84fd-2fac867ea86c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9eaeca53-2cfc-409c-a411-63bf7f69f8c6"));
        }
    }
}
