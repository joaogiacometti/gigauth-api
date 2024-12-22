using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Users;

public static class RequestCreateUserBuilder
{
    public static RequestCreateUser Build() => new Faker<RequestCreateUser>()
        .RuleFor(u => u.UserName, faker => faker.Internet.UserName().PadLeft(8, 'a'))
        .RuleFor(u => u.Email, faker => faker.Internet.Email())
        .RuleFor(u => u.Password, _ => PasswordBuilder.Build)
        .Generate();
}