using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class b : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate" },
                values: new object[] { new Guid("f574d33c-d8bf-4dec-9173-09b6580f25ab"), new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc), null, "Admin", new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[] { new Guid("f9210a4e-fdaf-4cb2-a1b0-18925b493d6a"), new Guid("f574d33c-d8bf-4dec-9173-09b6580f25ab"), new Guid("728e5486-ddd3-42cd-b8c5-3278181b1d36") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumn: "Id",
                keyValue: new Guid("f9210a4e-fdaf-4cb2-a1b0-18925b493d6a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f574d33c-d8bf-4dec-9173-09b6580f25ab"));
        }
    }
}
