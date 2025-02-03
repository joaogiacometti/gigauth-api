using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Users;

public static class RequestUpdateUserBuilder
{
    public static RequestUpdateUser Build()
    {
        return new Faker<RequestUpdateUser>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName().PadRight(8, 'a'))
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .Generate();
    }
}