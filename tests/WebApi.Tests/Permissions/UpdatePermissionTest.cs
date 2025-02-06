using System.Globalization;
using System.Net;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Permissions;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests.Permissions;

public class UpdatePermissionTest : GigAuthFixture
{
    private const string Method = "permission/update";
    private readonly string _adminToken;

    private readonly GigAuthContext _dbContext;
    private readonly string _userToken;

    public UpdatePermissionTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _dbContext = webApplicationFactory.DbContext;
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var permission = PermissionBuilder.Build();
        var request = new RequestPermission { Name = "newPermissionName" };
        await _dbContext.AddAsync(permission);
        await _dbContext.SaveChangesAsync();

        var result = await DoPut(Method, _adminToken, request, pathParameter: permission.Id.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NoContent);

        _dbContext.ChangeTracker.Clear();
        var updatedPermission = await _dbContext.Permissions.FirstAsync(u => u.Id.Equals(permission.Id));
        Assert.NotNull(updatedPermission);
        Assert.Equivalent(updatedPermission.Name, request.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var permission = PermissionBuilder.Build();
        var request = new RequestPermission { Name = "sh" };
        await _dbContext.AddAsync(permission);
        await _dbContext.SaveChangesAsync();

        var result = await DoPut(Method, _adminToken, request, culture, permission.Id.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.BadRequest);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("NAME_TOO_SHORT", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound(string culture)
    {
        var request = RequestPermissionBuilder.Build();
        var result = await DoPut(Method, _adminToken, request, culture, Guid.NewGuid().ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NotFound);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("PERMISSION_NOT_FOUND", new CultureInfo(culture))!;

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