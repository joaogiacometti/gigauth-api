using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PermissionManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedDate", "Description", "IsActive", "Name", "UpdatedDate" },
                values: new object[] { new Guid("6aa94319-e9ee-4d79-8cad-c6c3cb69520c"), new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc), null, true, "Permission", new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[] { new Guid("caab3f43-fdba-4568-8b92-27b8383ee64e"), new Guid("6aa94319-e9ee-4d79-8cad-c6c3cb69520c"), new Guid("9eaeca53-2cfc-409c-a411-63bf7f69f8c6") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: new Guid("caab3f43-fdba-4568-8b92-27b8383ee64e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6aa94319-e9ee-4d79-8cad-c6c3cb69520c"));
        }
    }
}
