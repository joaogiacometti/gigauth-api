using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using FluentAssertions;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Entities;

namespace WebApi.Tests.Users;

public class GetUserTest : GigAuthFixture
{
    private const string Method = "user/get";

    private readonly User _user;
    private readonly string _userToken;
    private readonly string _adminToken;

    public GetUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _user = webApplicationFactory.User.GetUser();
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(Method, token: _adminToken, pathParameter: _user.Id.ToString());

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await result.Deserialize<ResponseUser>(); 

        response.Should().NotBeNull();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoGet(Method, token: null, culture, _user.Id.ToString());

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoGet(Method, token: _userToken, culture, _user.Id.ToString());

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}