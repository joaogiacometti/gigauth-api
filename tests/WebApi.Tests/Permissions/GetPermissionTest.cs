using System.Globalization;
using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using GigAuth.Communication.Responses;
using GigAuth.Domain.Constants;
using GigAuth.Exception.Resources;

namespace WebApi.Tests.Permissions;

public class GetPermissionTest : GigAuthFixture
{
    private const string Method = "permission/get";
    private readonly string _adminToken;

    private readonly string _userToken;

    public GetPermissionTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(Method, _adminToken, pathParameter: RoleConstants.AdminPermissionId.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.OK);

        var response = await result.Deserialize<ResponsePermission>();

        Assert.NotNull(response);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound(string culture)
    {
        var result = await DoGet(Method, _adminToken, pathParameter: Guid.NewGuid().ToString(), culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NotFound);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("PERMISSION_NOT_FOUND", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoGet(Method, null, pathParameter: RoleConstants.AdminPermissionId.ToString(), culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoGet(Method, _userToken, pathParameter: RoleConstants.AdminPermissionId.ToString(), culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Forbidden);
    }
}