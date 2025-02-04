using GigAuth.Domain.Entities;

namespace WebApi.Tests.Resources;

public class UserIdentityManager(User user, string password, string token, string refreshToken)
{
    private readonly User _user = user;
    private readonly string _password = password;
    private readonly string _token = token;
    private readonly string _refreshToken = refreshToken;

    public User GetUser() => _user;

    public string GetPassword() => _password;

    public string GetToken() => _token;
    
    public string GetRefreshToken() => _refreshToken;
}