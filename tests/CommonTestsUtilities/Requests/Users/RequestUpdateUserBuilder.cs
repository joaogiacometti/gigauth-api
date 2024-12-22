using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Users;

public static class RequestUpdateUserBuilder
{
    public static RequestUpdateUser Build()
    {
        var faker = new Faker();

        return new RequestUpdateUser()
        {
            UserName = faker.Internet.UserName().PadRight(8, 'a'),
            Email = faker.Internet.Email()
        };
    }
}