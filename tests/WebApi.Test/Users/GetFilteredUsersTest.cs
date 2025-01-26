using System.Globalization;
using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using FluentAssertions;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Filters;
using GigAuth.Exception.Resources;

namespace WebApi.Test.Users;

public class GetFilteredUsersTest : GigAuthFixture
{
    private const string Method = "user/get-filtered";

    private readonly string _userToken;
    private readonly string _adminToken;

    public GetFilteredUsersTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoPost(Method, new object(), _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await result.Deserialize<List<ResponseUser>>(); 

        response.Should().NotBeNull();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoPost(Method, new object(), null, culture);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoPost(Method, new object(), _userToken, culture);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var result = await DoPost(Method, new RequestUserFilter() {UserName = ""}, _adminToken, culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NAME_EMPTY", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }
}