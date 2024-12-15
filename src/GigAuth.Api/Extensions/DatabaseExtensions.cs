using GigAuth.Infrastructure.Migrations;

namespace GigAuth.Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task DatabaseMigrate(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
    }
}