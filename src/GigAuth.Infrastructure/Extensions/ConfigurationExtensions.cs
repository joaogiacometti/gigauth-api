using Microsoft.Extensions.Configuration;

namespace GigAuth.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static bool IsTestEnvironment(this IConfiguration configuration) => configuration.GetValue<bool>("InMemoryTest");
}