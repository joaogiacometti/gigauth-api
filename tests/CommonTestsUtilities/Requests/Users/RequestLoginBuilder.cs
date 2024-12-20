using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Users;

public static class RequestLoginBuilder
{
    public static RequestLogin Build()
    {
        var faker = new Faker();

        return new RequestLogin()
        {
            Email = faker.Internet.Email(),
            Password = PasswordBuilder.Build,
        };
    }
}