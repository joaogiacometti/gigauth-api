using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebWebApi.Test;

public class GigAuthFixture(CustomWebApplicationFactory webApplicationFactory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();
    
    protected async Task<HttpResponseMessage> DoPost(string requestUri, object request, string token = "", string culture = "en")
    {
        SetRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }
    
    private void SetRequestCulture(string culture)
    {
        if (string.IsNullOrWhiteSpace(culture)) return;
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}