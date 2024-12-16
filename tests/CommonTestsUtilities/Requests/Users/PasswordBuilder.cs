namespace CommonTestsUtilities.Requests.Users;
using Bogus;

public static class PasswordBuilder
{
    public static string Build => new Faker().Internet.Password(prefix: "Aa1!");
}