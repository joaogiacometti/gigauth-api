using Bogus;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class ForgotPasswordTokenBuilder
{
    public static ForgotPasswordToken Build()
    {
        return new Faker<ForgotPasswordToken>()
            .RuleFor(fpt => fpt.ExpirationDate, faker => faker.Date.Future())
            .RuleFor(fpt => fpt.Token, _ => Guid.NewGuid().ToString())
            .Generate();
    }
}