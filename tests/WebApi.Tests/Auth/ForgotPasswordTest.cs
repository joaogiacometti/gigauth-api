using System.Globalization;
using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using GigAuth.Domain.Entities;
using GigAuth.Exception.Resources;

namespace WebApi.Tests.Auth;

public class ForgotPasswordTest : GigAuthFixture
{
    private const string Method = "auth/forgot-password";

    private readonly User _user;

    public ForgotPasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _user = webApplicationFactory.User.GetUser();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoPost(Method, pathParameter: _user.UserName);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_User_Not_Found(string culture)
    {
        var result = await DoPost(Method, pathParameter: "invalid", culture: culture);
    
        Assert.Equivalent(result.StatusCode, HttpStatusCode.NotFound);
    
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture))!;
    
        await result.CompareException(expectedMessage);
    }
}