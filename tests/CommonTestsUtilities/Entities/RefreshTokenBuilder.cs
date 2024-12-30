using Bogus;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class RefreshTokenBuilder
{
    public static RefreshToken Build(Guid userId) => new Faker<RefreshToken>()
        .RuleFor(rt => rt.UserId, _ => userId)
        .RuleFor(rt => rt.ExpirationDate, faker => faker.Date.Future())
        .Generate();
}