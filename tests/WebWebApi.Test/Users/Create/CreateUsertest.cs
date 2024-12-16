using System.Globalization;
using System.Net;
using System.Text.Json;
using Bogus;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Users;
using FluentAssertions;
using GigAuth.Exception.Resources;

namespace WebWebApi.Test.Users.Create;

public class CreateUsertest(CustomWebApplicationFactory webApplicationFactory) : GigAuthFixture(webApplicationFactory)
{
    private const string Method = "/Users";
    
    [Fact]
    public async Task Success()
    {
        var request = RequestCreateUserBuilder.Build();

        var result = await DoPost(Method, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_UserName_Already_Used(string culture)
    {
        var request = RequestCreateUserBuilder.Build();

        var firstResult = await DoPost(Method, request, culture: culture);
        var secondResult = await DoPost(Method, request, culture: culture);
        
        firstResult.StatusCode.Should().Be(HttpStatusCode.Created);
        secondResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await secondResult.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errorList = response.RootElement.GetProperty("ErrorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NAME_ALREADY_USED", new CultureInfo(culture));

        errorList.Should().Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Already_Used(string culture)
    {
        var request = RequestCreateUserBuilder.Build();

        var firstResult = await DoPost(Method, request, culture: culture);
        request.UserName = new Faker().Internet.UserName();
        var secondResult = await DoPost(Method, request, culture: culture);
        
        firstResult.StatusCode.Should().Be(HttpStatusCode.Created);
        secondResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await secondResult.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errorList = response.RootElement.GetProperty("ErrorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_INVALID", new CultureInfo(culture));

        errorList.Should().Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_InvalidRequest(string culture)
    {
        var request = RequestCreateUserBuilder.Build();
        request.UserName = string.Empty;

        var result = await DoPost(Method, request, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errorList = response.RootElement.GetProperty("ErrorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NAME_EMPTY", new CultureInfo(culture));

        errorList.Should().Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}