using System.Globalization;
using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using CommonTestsUtilities.Requests.Common;
using GigAuth.Communication.Requests;
using GigAuth.Domain.Entities;
using GigAuth.Exception.Resources;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests.Auth;

public class ChangePasswordTest : GigAuthFixture
{
    private const string Method = "auth/change-password";

    private readonly User _user;

    public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _user = webApplicationFactory.User.GetUser();
    }

    [Fact]
    public async Task Success()
    {
        var previousPassword = _user.PasswordHash;
        
        var forgotResult = await DoPost("auth/forgot-password", pathParameter: _user.UserName);
        
        Assert.Equivalent(forgotResult.StatusCode, HttpStatusCode.NoContent);
        
        var token = await DbContext.ForgotPasswordTokens.FirstAsync(fpt => fpt.UserId == _user.Id);
        
        var request = new RequestChangePassword { Token = token.Token, NewPassword = PasswordBuilder.Build};

        var changeResult = await DoPut(Method, null, request);
        
        Assert.Equivalent(changeResult.StatusCode, HttpStatusCode.NoContent);
        
        DbContext.ChangeTracker.Clear();
        
        var user = await DbContext.Users.FirstAsync(u => u.Id.Equals(_user.Id));
        
        Assert.NotEqual(previousPassword, user.PasswordHash);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Not_Found(string culture)
    {
        var request = new RequestChangePassword { Token = "not exists", NewPassword = PasswordBuilder.Build};

        var result = await DoPut(Method, null, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.NotFound);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("TOKEN_NOT_FOUND", new CultureInfo(culture))!;
        
        await result.CompareException(expectedMessage);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var request = RequestChangePasswordBuilder.Build();
        request.Token = string.Empty;
    
        var result = await DoPut(Method, null, request, culture);
    
        Assert.Equivalent(result.StatusCode, HttpStatusCode.BadRequest);
    
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("TOKEN_EMPTY", new CultureInfo(culture))!;
    
        await result.CompareException(expectedMessage);
    }
}