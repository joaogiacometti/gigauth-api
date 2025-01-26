using System.Globalization;
using System.Net;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using FluentAssertions;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests.Users;

public class UpdateUserTest : GigAuthFixture
{
    private const string Method = "user/update";

    private readonly GigAuthContext _dbContext;
    private readonly string _userToken;
    private readonly string _adminToken;

    public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _dbContext = webApplicationFactory.DbContext;
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = new RequestUpdateUser() { UserName = "newUserName"};
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        var result = await DoPut(Method, token: _adminToken, request, pathParameter: user.Id.ToString());

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        _dbContext.ChangeTracker.Clear();
        var updatedUser = await _dbContext.Users.FirstAsync(u => u.Id.Equals(user.Id));
        updatedUser.Should().NotBeNull();
        updatedUser.UserName.Should().Be(request.UserName);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var user = UserBuilder.Build();
        var request = new RequestUpdateUser() { UserName = "short"};
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        var result = await DoPut(Method, token: _adminToken, request, culture, user.Id.ToString());

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NAME_TOO_SHORT", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound(string culture)
    {
        var result = await DoPut(Method, _adminToken, culture, Guid.NewGuid().ToString());

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoPut(Method, null, new object(), culture, Guid.NewGuid().ToString());

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoPut(Method, _userToken, new object(), culture, Guid.NewGuid().ToString());

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}