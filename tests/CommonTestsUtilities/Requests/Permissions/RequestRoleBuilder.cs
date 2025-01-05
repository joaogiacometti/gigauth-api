using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Permissions;

public static class RequestPermissionBuilder
{
    public static RequestPermission Build() => new Faker<RequestPermission>()
        .RuleFor(u => u.Name, faker => faker.Lorem.Word().PadLeft(3, 'a'))
        .RuleFor(u => u.Description, faker => faker.Random.Bool() 
            ? faker.Lorem.Sentence(wordCount: 3) 
            : null)
        .Generate();
}