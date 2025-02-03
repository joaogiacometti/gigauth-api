using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Auth;

public static class RequestRefreshTokenBuilder
{
    public static RequestRefreshToken Build(string? refreshToken = null)
    {
        return new Faker<RequestRefreshToken>()
            .RuleFor(u => u.Token, faker => faker.Random.AlphaNumeric(100))
            .RuleFor(u => u.RefreshToken, faker => refreshToken ?? faker.Random.AlphaNumeric(100))
            .Generate();
    }
}