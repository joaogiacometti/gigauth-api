using Bogus;
using CommonTestsUtilities.Requests.Users;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class RefreshTokenBuilder
{
    public static RefreshToken Build(Guid userId) => new Faker<RefreshToken>()
        .RuleFor(u => u.UserId, _ => userId)
        .Generate();
}