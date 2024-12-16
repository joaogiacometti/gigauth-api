using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Users;

public static class RequestCreateUserBuilder
{
    public static RequestCreateUser Build()
    {
        var faker = new Faker();

        return new RequestCreateUser()
        {
            UserName = faker.Internet.UserName().PadRight(8, 'a'),
            Email = faker.Internet.Email(),
            Password = PasswordBuilder.Build,
        };
    }
}