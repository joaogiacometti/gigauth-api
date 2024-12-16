using Bogus;
using CommonTestsUtilities.Requests.Users;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class UserBuilder
{
    public static User Build()
    {
        var faker = new Faker();

        return new User()
        {
            Id = Guid.NewGuid(),
            UserName = faker.Internet.UserName(),
            Email = faker.Internet.Email(),
            PasswordHash = PasswordBuilder.Build,
            IsActive = true,
        };
    }
}