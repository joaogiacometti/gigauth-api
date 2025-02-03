using System.Globalization;
using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Entities;
using GigAuth.Exception.Resources;

namespace WebApi.Tests.Auth;

public class LoginTest : GigAuthFixture
{
    private const string Method = "auth/login";

    private readonly User _user;
    private readonly string _userPassword;

    public LoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _user = webApplicationFactory.User.GetUser();
        _userPassword = webApplicationFactory.User.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLogin { Email = _user.Email, Password = _userPassword };

        var result = await DoPost(Method, request);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.OK);

        var response = await result.Deserialize<ResponseToken>();

        Assert.NotNull(response?.Token);
        Assert.NotEmpty(response.Token);
        Assert.NotNull(response.RefreshToken);
        Assert.NotEmpty(response.Token);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Not_Found(string culture)
    {
        var request = new RequestLogin { Email = "not@exists.com", Password = _userPassword };

        var result = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("CREDENTIALS_INVALID", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Password(string culture)
    {
        var request = new RequestLogin { Email = _user.Email, Password = "InvalidPassword123!" };

        var result = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("CREDENTIALS_INVALID", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var request = RequestLoginBuilder.Build();
        request.Email = "invalid_email";

        var result = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("CREDENTIALS_INVALID", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }
}