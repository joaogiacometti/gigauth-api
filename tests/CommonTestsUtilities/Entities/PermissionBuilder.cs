using Bogus;
using CommonTestsUtilities.Requests.Users;
using GigAuth.Domain.Entities;

namespace CommonTestsUtilities.Entities;

public static class PermissionBuilder
{
    public static Permission Build() => new Faker<Permission>()
        .RuleFor(u => u.Id, _ => Guid.NewGuid())
        .RuleFor(u => u.Name, faker => faker.Internet.UserName().PadLeft(8, 'a'))
        .RuleFor(u => u.Description, faker => faker.Internet.Email())
        .RuleFor(u => u.IsActive, _ => true).Generate();
    
    public static List<Permission> BuildList(int qtd = 5) => Enumerable.Range(0, qtd).Select(_ => Build()).ToList();
}