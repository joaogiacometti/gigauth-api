using GigAuth.Api.Extensions;
using GigAuth.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.ConfigureDependencies(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if(builder.Configuration.IsTestEnvironment() == false)
    await app.DatabaseMigrate();

app.ConfigureMiddlewares();
app.ConfigureEndpoints();

app.Run();