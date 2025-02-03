using Bogus;
using CommonTestsUtilities.Requests.Common;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Auth;

public static class RequestLoginBuilder
{
    public static RequestLogin Build()
    {
        return new Faker<RequestLogin>()
            .RuleFor(u => u.Email, faker => faker.Internet.Email())
            .RuleFor(u => u.Password, PasswordBuilder.Build)
            .Generate();
    }
}