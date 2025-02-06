using System.Globalization;
using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Constants;
using GigAuth.Exception.Resources;

namespace WebApi.Tests.Roles;

public class GetRoleTest : GigAuthFixture
{
    private const string Method = "role/get";
    private readonly string _adminToken;

    private readonly string _userToken;

    public GetRoleTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(Method, _adminToken, pathParameter: RoleConstants.AdminRoleId.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.OK);

        var response = await result.Deserialize<ResponseRole>();

        Assert.NotNull(response);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound(string culture)
    {
        var result = await DoGet(Method, _adminToken, pathParameter: Guid.NewGuid().ToString(), culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NotFound);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("ROLE_NOT_FOUND", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoGet(Method, null, pathParameter: RoleConstants.AdminRoleId.ToString(), culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoGet(Method, _userToken, pathParameter: RoleConstants.AdminRoleId.ToString(), culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Forbidden);
    }
}