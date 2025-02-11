using GigAuth.Api.Auth;
using GigAuth.Api.Extensions;
using GigAuth.Domain.Constants;
using GigAuth.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.ConfigureRateLimiter();

builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());
builder.Services.ConfigureDependencies(configuration);

builder.AddJwtAuth();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.BluePlanet);
        options.WithHttpBearerAuthentication(bearer => { bearer.Token = "your-bearer-token"; });
    });

    app.UseCors("dev");
}

if (!configuration.IsTestEnvironment())
{
    await app.DatabaseMigrate();
    app.MapHealthChecks("/_health")
        .RequireRateLimiting(RateLimiterConstants.Global);
}

app.ConfigureMiddlewares();
app.ConfigureEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

await app.RunAsync();

public partial class Program { }