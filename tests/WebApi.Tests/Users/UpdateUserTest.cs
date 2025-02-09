using System.Globalization;
using System.Net;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Users;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests.Users;

public class UpdateUserTest : GigAuthFixture
{
    private const string Method = "user/update";
    private readonly string _adminToken;

    private readonly string _userToken;

    public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = new RequestUpdateUser { UserName = "newUserName" };
        await DbContext.AddAsync(user);
        await DbContext.SaveChangesAsync();

        var result = await DoPut(Method, _adminToken, request, pathParameter: user.Id.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NoContent);

        DbContext.ChangeTracker.Clear();
        var updatedUser = await DbContext.Users.FirstAsync(u => u.Id.Equals(user.Id));
        Assert.NotNull(updatedUser);
        Assert.Equivalent(updatedUser.UserName, request.UserName);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var user = UserBuilder.Build();
        var request = new RequestUpdateUser { UserName = "short" };
        await DbContext.AddAsync(user);
        await DbContext.SaveChangesAsync();

        var result = await DoPut(Method, _adminToken, request, culture, user.Id.ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.BadRequest);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("USER_NAME_TOO_SHORT", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound(string culture)
    {
        var request = RequestUpdateUserBuilder.Build();
        var result = await DoPut(Method, _adminToken, request, culture, Guid.NewGuid().ToString());

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NotFound);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture))!;

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