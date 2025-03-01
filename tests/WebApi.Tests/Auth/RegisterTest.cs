using System.Globalization;
using System.Net;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Auth;
using GigAuth.Exception.Resources;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests.Auth;

public class RegisterTest : GigAuthFixture
{
    private const string Method = "auth/register";

    public RegisterTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    { }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterBuilder.Build();

        var result = await DoPost(Method, request);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Created);

        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);

        Assert.NotNull(user);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Username_Already_Used(string culture)
    {
        var request = RequestRegisterBuilder.Build();

        var firstTry = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(firstTry.StatusCode, HttpStatusCode.Created);

        var secondTry = await DoPost(Method, request, culture: culture);
        
        Assert.Equivalent(secondTry.StatusCode, HttpStatusCode.Conflict);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("USER_NAME_ALREADY_USED", new CultureInfo(culture))!;

        await secondTry.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Already_Used(string culture)
    {
        var request = RequestRegisterBuilder.Build();
        var user = UserBuilder.Build();
        user.Email = request.Email;

        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();

        var result = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.BadRequest);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var request = RequestRegisterBuilder.Build();
        request.UserName = "short";

        var result = await DoPost(Method, request, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.BadRequest);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("USER_NAME_TOO_SHORT", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }
}