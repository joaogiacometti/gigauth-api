using GigAuth.Domain.Entities;

namespace WebApi.Tests.Resources;

public class UserIdentityManager(User user, string password, string token)
{
    private readonly string _password = password;
    private readonly string _token = token;
    private readonly User _user = user;

    public User GetUser()
    {
        return _user;
    }

    public string GetPassword()
    {
        return _password;
    }

    public string GetToken()
    {
        return _token;
    }
}