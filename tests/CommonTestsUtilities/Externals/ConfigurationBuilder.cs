using Microsoft.Extensions.Configuration;
using Moq;

namespace CommonTestsUtilities.Externals;

public class ConfigurationBuilder
{
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<IConfigurationSection> _configurationSection = new();

    public ConfigurationBuilder ForgotPasswordTime(int time)
    {
        _configurationSection.Setup(x => x.Value).Returns(time.ToString());
        _configuration.Setup(x => x.GetSection("ForgotPasswordToken:ExpirationInSeconds")).Returns(_configurationSection.Object);
        return this;
    }
    public IConfiguration Build() => _configuration.Object;
}