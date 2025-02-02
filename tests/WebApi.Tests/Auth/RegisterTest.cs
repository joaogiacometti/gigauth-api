using System.Globalization;
using System.Net;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using FluentAssertions;
using GigAuth.Exception.Resources;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests.Auth;

public class RegisterTest : GigAuthFixture
{
    private const string Method = "auth/register";

    private readonly GigAuthContext _dbContext;

    public RegisterTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _dbContext = webApplicationFactory.DbContext;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterBuilder.Build();
        
        var result = await DoPost(Method, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
        user.Should().NotBeNull();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Username_Already_Used(string culture)
    {
        var request = RequestRegisterBuilder.Build();
        var user = UserBuilder.Build();
        user.UserName = request.UserName;

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        var result = await DoPost(Method, request, culture: culture);
    
        result.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NAME_ALREADY_USED", new CultureInfo(culture))!;
    
        await result.CompareException(expectedMessage);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Already_Used(string culture)
    {
        var request = RequestRegisterBuilder.Build();
        var user = UserBuilder.Build();
        user.Email = request.Email;

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        var result = await DoPost(Method, request, culture: culture);
    
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture))!;
    
        await result.CompareException(expectedMessage);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var request = RequestRegisterBuilder.Build();
        request.UserName = "short";
        
        var result = await DoPost(Method, request, culture: culture);
    
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NAME_TOO_SHORT", new CultureInfo(culture))!;
    
        await result.CompareException(expectedMessage);
    }
}