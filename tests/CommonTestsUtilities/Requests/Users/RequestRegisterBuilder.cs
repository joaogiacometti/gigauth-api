using Bogus;
using CommonTestsUtilities.Requests.Common;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Users;

public static class RequestRegisterBuilder
{
    public static RequestRegister Build() => new Faker<RequestRegister>()
        .RuleFor(u => u.UserName, faker => faker.Internet.UserName().PadLeft(8, 'a'))
        .RuleFor(u => u.Email, faker => faker.Internet.Email())
        .RuleFor(u => u.Password, _ => PasswordBuilder.Build)
        .Generate();
}