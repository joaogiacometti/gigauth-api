using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Permissions;

public static class RequestPermissionBuilder
{
    public static RequestPermission Build()
    {
        return new Faker<RequestPermission>()
            .RuleFor(u => u.Name, faker => faker.Lorem.Word().PadLeft(3, 'a'))
            .RuleFor(u => u.Description, faker => faker.Random.Bool()
                ? faker.Lorem.Sentence(3)
                : null)
            .Generate();
    }
}