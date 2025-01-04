using GigAuth.Api.Auth;
using GigAuth.Api.Extensions;
using GigAuth.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());
builder.Services.ConfigureDependencies(configuration);

builder.AddJwtAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.BluePlanet);
        options.WithHttpBearerAuthentication(bearer => { bearer.Token = "your-bearer-token"; });
    });
}

if (!configuration.IsTestEnvironment())
    await app.DatabaseMigrate();

app.ConfigureMiddlewares();
app.ConfigureEndpoints();

// TODO: Add rate limiting
app.MapHealthChecks("/_health");

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();

public abstract partial class Program { }