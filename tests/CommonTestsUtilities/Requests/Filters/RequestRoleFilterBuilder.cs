using Bogus;
using GigAuth.Domain.Filters;

namespace CommonTestsUtilities.Requests.Filters;

public static class RequestRoleFilterBuilder
{
    public static RequestRoleFilter Build()
    {
        return new Faker<RequestRoleFilter>()
            .RuleFor(u => u.Name, f => f.Internet.UserName().PadRight(8, 'a'))
            .RuleFor(u => u.Description, f => f.Internet.Email())
            .RuleFor(u => u.IsActive, f => f.Random.Bool())
            .Generate();
    }
}