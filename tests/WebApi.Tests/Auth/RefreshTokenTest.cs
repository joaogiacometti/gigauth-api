using System.Globalization;
using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using GigAuth.Communication.Requests;
using GigAuth.Communication.Responses;
using GigAuth.Exception.Resources;

namespace WebApi.Tests.Auth;

public class RefreshTokenTest : GigAuthFixture
{
    private const string Method = "auth/refresh-token";

    private readonly string _token;
    private readonly string _refreshToken;

    public RefreshTokenTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _refreshToken = webApplicationFactory.User.GetRefreshToken();
    }
    
    [Fact]
    public async Task Success()
    {
        var request = new RequestRefreshToken() { Token = _token, RefreshToken = _refreshToken};

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
    public async Task Error_Invalid_Token(string culture)
    {
        var request = new RequestRefreshToken() { Token = _token, RefreshToken = _refreshToken};
        request.Token = "invalid";

        var result = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("TOKEN_INVALID", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_RefreshToken_Not_Found(string culture)
    {
        var request = new RequestRefreshToken() { Token = _token, RefreshToken = _refreshToken};
        request.RefreshToken = "invalid";

        var result = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("REFRESH_TOKEN_INVALID", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var request = new RequestRefreshToken() { Token = _token, RefreshToken = _refreshToken};
        request.Token = string.Empty;

        var result = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.BadRequest);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("TOKEN_EMPTY", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }
}