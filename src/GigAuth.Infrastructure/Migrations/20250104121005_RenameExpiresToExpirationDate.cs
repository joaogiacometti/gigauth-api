using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameExpiresToExpirationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expires",
                table: "ForgotPasswordTokens",
                newName: "ExpirationDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "ForgotPasswordTokens",
                newName: "Expires");
        }
    }
}
