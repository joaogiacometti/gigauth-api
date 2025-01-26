using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Tests;

public class GigAuthFixture(CustomWebApplicationFactory webApplicationFactory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();
    
    protected async Task<HttpResponseMessage> DoPost(string requestUri, object request, string? token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        SetRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }
    
    protected async Task<HttpResponseMessage> DoGet (string requestUri, string? token, string culture = "en")
    {
        AuthorizeRequest(token);
        SetRequestCulture(culture);

        return await _httpClient.GetAsync(requestUri);
    }
    
    private void AuthorizeRequest(string? token)
    {
        if (!string.IsNullOrWhiteSpace(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void SetRequestCulture(string culture)
    {
        if (string.IsNullOrWhiteSpace(culture)) return;
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}