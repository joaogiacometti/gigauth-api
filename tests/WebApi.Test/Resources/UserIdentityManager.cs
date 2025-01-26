using GigAuth.Domain.Entities;

namespace WebApi.Test.Resources;

public class UserIdentityManager(User user, string password, string token)
{
    private readonly User _user = user;
    private readonly string _password = password;
    private readonly string _token = token;

    public User GetUser() => _user;
    public string GetPassword() => _password;
    public string GetToken() => _token;
}