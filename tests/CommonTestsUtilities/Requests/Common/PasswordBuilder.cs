using Bogus;

namespace CommonTestsUtilities.Requests.Common;

public static class PasswordBuilder
{
    public static string Build => new Faker().Internet.Password(prefix: "Aa1!");
}