using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Roles;

public static class RequestRoleBuilder
{
    public static RequestRole Build()
    {
        return new Faker<RequestRole>()
            .RuleFor(u => u.Name, faker => faker.Lorem.Word().PadLeft(3, 'a'))
            .RuleFor(u => u.Description, faker => faker.Random.Bool()
                ? faker.Lorem.Sentence(3)
                : null)
            .Generate();
    }
}