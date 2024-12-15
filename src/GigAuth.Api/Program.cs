using Scalar.AspNetCore;
using GigAuth.Infrastructure;
using GigAuth.Infrastructure.Extensions;
using GigAuth.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if(builder.Configuration.IsTestEnvironment() == false)
    await MigrateDatabase();

app.Run();

return;

async Task MigrateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();

    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}