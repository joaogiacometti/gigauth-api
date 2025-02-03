using Bogus;
using CommonTestsUtilities.Requests.Common;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Auth;

public static class RequestChangePasswordBuilder
{
    public static RequestChangePassword Build()
    {
        return new Faker<RequestChangePassword>()
            .RuleFor(cp => cp.NewPassword, _ => PasswordBuilder.Build)
            .RuleFor(cp => cp.Token, _ => Guid.NewGuid().ToString())
            .Generate();
    }
}