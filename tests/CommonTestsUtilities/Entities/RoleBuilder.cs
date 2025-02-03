using Bogus;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class RoleBuilder
{
    public static Role Build()
    {
        return new Faker<Role>()
            .RuleFor(u => u.Id, _ => Guid.NewGuid())
            .RuleFor(u => u.Name, faker => faker.Internet.UserName().PadLeft(8, 'a'))
            .RuleFor(u => u.Description, faker => faker.Internet.Email())
            .RuleFor(u => u.IsActive, _ => true).Generate();
    }

    public static List<Role> BuildList(int qtd = 5)
    {
        return Enumerable.Range(0, qtd).Select(_ => Build()).ToList();
    }
}