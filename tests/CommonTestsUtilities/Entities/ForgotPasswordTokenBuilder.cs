using Bogus;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class ForgotPasswordTokenBuilder
{
    public static ForgotPasswordToken Build() => new Faker<ForgotPasswordToken>()
        .RuleFor(fpt => fpt.Expires, faker => faker.Date.Future())
        .Generate();
}