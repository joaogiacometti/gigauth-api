using Bogus;
using GigAuth.Communication.Requests;

namespace CommonTestsUtilities.Requests.Roles;

public static class RequestCreateRoleBuilder
{
    public static RequestCreateRole Build() => new Faker<RequestCreateRole>()
        .RuleFor(u => u.Name, faker => faker.Lorem.Word().PadLeft(3, 'a'))
        .RuleFor(u => u.Description, faker => faker.Random.Bool() 
            ? faker.Lorem.Sentence(wordCount: 3) 
            : null)
        .Generate();
}