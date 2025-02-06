using System.Globalization;
using System.Net;
using CommonTestsUtilities.Extensions;
using CommonTestsUtilities.InlineData;
using CommonTestsUtilities.Requests.Permissions;
using GigAuth.Exception.Resources;

namespace WebApi.Tests.Permissions;

public class CreatePermissionTest: GigAuthFixture
{
    private const string Method = "permission/create";
    private readonly string _adminToken;
    private readonly string _userToken;
    
    public CreatePermissionTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.Admin.GetToken();
        _userToken = webApplicationFactory.User.GetToken();
    }
    
    [Fact]
    public async Task Success()
    {
        var request = RequestPermissionBuilder.Build();

        var result = await DoPost(Method, request, _adminToken);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Created);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Request(string culture)
    {
        var request = RequestPermissionBuilder.Build();
        request.Name = "a";

        var result = await DoPost(Method, request, _adminToken, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.BadRequest);

        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("NAME_TOO_SHORT", new CultureInfo(culture))!;

        await result.CompareException(expectedMessage);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Already_Used(string culture)
    {
        var request = RequestPermissionBuilder.Build();

        var firstTry = await DoPost(Method, request, _adminToken, culture: culture);

        Assert.Equivalent(firstTry.StatusCode, HttpStatusCode.Created);
        
        var secondTry = await DoPost(Method, request, _adminToken, culture: culture);
        
        Assert.Equal(HttpStatusCode.Conflict, secondTry.StatusCode);
        
        var expectedMessage =
            ResourceErrorMessages.ResourceManager.GetString("NAME_ALREADY_USED", new CultureInfo(culture))!;

        await secondTry.CompareException(expectedMessage);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized(string culture)
    {
        var request = RequestPermissionBuilder.Build();
        
        var result = await DoPost(Method, request, null, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Forbidden(string culture)
    {
        var request = RequestPermissionBuilder.Build();
        
        var result = await DoPost(Method, request, _userToken, culture: culture);

        Assert.Equivalent(result.StatusCode, HttpStatusCode.Forbidden);
    }
}