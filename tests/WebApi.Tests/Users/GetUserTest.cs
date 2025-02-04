using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Entities;

namespace WebApi.Tests.Users;

public class GetUserTest : GigAuthFixture
{
    private const string Method = "user/get";
    private readonly string _adminToken;

    private readonly User _user;
    private readonly string _userToken;

    public GetUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _user = webApplicationFactory.User.GetUser();
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(Method, _adminToken, pathParameter: _user.Id.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.OK);

        var response = await result.Deserialize<ResponseUser>();

        Assert.NotNull(response);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoGet(Method, null, pathParameter: _user.Id.ToString(), culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoGet(Method, _userToken, pathParameter: _user.Id.ToString(), culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Forbidden);
    }
}