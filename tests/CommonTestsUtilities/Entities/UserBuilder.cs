using Bogus;
using CommonTestsUtilities.Requests.Users;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class UserBuilder
{
    public static User Build() => new Faker<User>()
        .RuleFor(u => u.Id, _ => Guid.NewGuid())
        .RuleFor(u => u.UserName, faker => faker.Internet.UserName().PadLeft(8, 'a'))
        .RuleFor(u => u.Email, faker => faker.Internet.Email())
        .RuleFor(u => u.PasswordHash, _ => PasswordBuilder.Build)
        .RuleFor(u => u.IsActive, _ => true).Generate();
}