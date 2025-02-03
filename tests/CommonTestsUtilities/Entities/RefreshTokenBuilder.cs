using Bogus;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class RefreshTokenBuilder
{
    public static RefreshToken Build(Guid? userId = null)
    {
        return new Faker<RefreshToken>()
            .RuleFor(rt => rt.Id, _ => Guid.NewGuid())
            .RuleFor(rt => rt.UserId, _ => userId ?? Guid.NewGuid())
            .RuleFor(rt => rt.Token, faker => faker.Random.AlphaNumeric(50))
            .RuleFor(rt => rt.ExpirationDate, faker => faker.Date.Future())
            .Generate();
    }
}