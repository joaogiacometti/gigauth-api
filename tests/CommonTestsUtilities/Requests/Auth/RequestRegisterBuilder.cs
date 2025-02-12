using Bogus;
using CommonTestsUtilities.Requests.Common;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Auth;

public static class RequestRegisterBuilder
{
    public static RequestRegister Build()
    {
        return new Faker<RequestRegister>()
            .RuleFor(u => u.UserName, faker => faker.Internet.UserName().PadLeft(8, 'a'))
            .RuleFor(u => u.Email, faker => faker.Internet.Email())
            .RuleFor(u => u.Password, _ => PasswordBuilder.Build)
            .RuleFor(u => u.PasswordConfirmation, (_, u) => u.Password)
            .Generate();
    }
}