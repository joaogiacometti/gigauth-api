using System.Globalization;
using System.Net;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Roles;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests.Roles;

public class UpdateRoleTest : GigAuthFixture
{
    private const string Method = "role/update";
    private readonly string _adminToken;

    private readonly GigAuthContext _dbContext;
    private readonly string _userToken;

    public UpdateRoleTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _dbContext = webApplicationFactory.DbContext;
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var role = RoleBuilder.Build();
        var request = new RequestRole { Name = "newRoleName" };
        await _dbContext.AddAsync(role);
        await _dbContext.SaveChangesAsync();

        var result = await DoPut(Method, _adminToken, request, pathParameter: role.Id.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NoContent);

        _dbContext.ChangeTracker.Clear();
        var updatedRole = await _dbContext.Roles.FirstAsync(u => u.Id.Equals(role.Id));
        Assert.NotNull(updatedRole);
        Assert.Equivalent(updatedRole.Name, request.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var role = RoleBuilder.Build();
        var request = new RequestRole { Name = "sh" };
        await _dbContext.AddAsync(role);
        await _dbContext.SaveChangesAsync();

        var result = await DoPut(Method, _adminToken, request, culture, role.Id.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.BadRequest);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("NAME_TOO_SHORT", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound(string culture)
    {
        var request = RequestRoleBuilder.Build();
        var result = await DoPut(Method, _adminToken, request, culture, Guid.NewGuid().ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NotFound);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("ROLE_NOT_FOUND", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoPut(Method, null, new object(), culture, Guid.NewGuid().ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoPut(Method, _userToken, new object(), culture, Guid.NewGuid().ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Forbidden);
    }
}