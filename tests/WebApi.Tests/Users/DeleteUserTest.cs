using System.Net;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.InlineData;
using FluentAssertions;
using GigAuth.Infrastructure.DataAccess;

namespace WebApi.Tests.Users;

public class DeleteUserTest : GigAuthFixture
{
    private const string Method = "user/delete";

    private readonly GigAuthContext _dbContext;
    private readonly string _userToken;
    private readonly string _adminToken;

    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _dbContext = webApplicationFactory.DbContext;
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        var firstTry = await DoDelete(Method, token: _adminToken, pathParameter: user.Id.ToString());

        firstTry.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var secondTry = await DoDelete(Method, token: _adminToken, pathParameter: user.Id.ToString());
        
        secondTry.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NotFound(string culture)
    {
        var result = await DoDelete(Method, _adminToken, culture, Guid.NewGuid().ToString());

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var result = await DoDelete(Method, null, culture, Guid.NewGuid().ToString());

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var result = await DoDelete(Method, _userToken, culture, Guid.NewGuid().ToString());

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}