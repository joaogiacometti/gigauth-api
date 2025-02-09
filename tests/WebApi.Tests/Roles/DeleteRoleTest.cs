using System.Globalization;
using System.Net;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using GigAuth.Exception.Resources;
using GigAuth.Infrastructure.DataAccess;

namespace WebApi.Tests.Roles;

public class DeleteRoleTest : GigAuthFixture
{
    private const string Method = "role/delete";
    private readonly string _adminToken;
    private readonly string _userToken;

    public DeleteRoleTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var role = RoleBuilder.Build();
        await DbContext.AddAsync(role);
        await DbContext.SaveChangesAsync();

        var firstTry = await DoDelete(Method, _adminToken, pathParameter: role.Id.ToString());

        Assert.Equivalent(firstTry.StatusCode, HttpStatusCode.NoContent);

        var secondTry = await DoDelete(Method, _adminToken, pathParameter: role.Id.ToString());

        Assert.Equivalent(secondTry.StatusCode, HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound(string culture)
    {
        var result = await DoDelete(Method, _adminToken, culture, Guid.NewGuid().ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NotFound);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("ROLE_NOT_FOUND", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoDelete(Method, null, culture, Guid.NewGuid().ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoDelete(Method, _userToken, culture, Guid.NewGuid().ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Forbidden);
    }
}