using System.Net.Http.Headers;
using System.Net.Http.Json;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests;

public class GigAuthFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    protected readonly GigAuthContext DbContext;

    protected GigAuthFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        var scope = webApplicationFactory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<GigAuthContext>();
    }

    protected async Task<HttpResponseMessage> DoPost(string requestUri, object? request = null, string? token = "",
        string? pathParameter = null, string? culture = "en")
    {
        AuthorizeRequest(token);
        SetRequestCulture(culture);
        
        if (!string.IsNullOrWhiteSpace(pathParameter))
            requestUri = $"{requestUri}/{pathParameter}";

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string requestUri, string? token, string? pathParameter = null, string culture = "en")
    {
        AuthorizeRequest(token);
        SetRequestCulture(culture);

        if (!string.IsNullOrWhiteSpace(pathParameter))
            requestUri = $"{requestUri}/{pathParameter}";

        return await _httpClient.GetAsync(requestUri);
    }

    protected async Task<HttpResponseMessage> DoPut<T>(string requestUri, string? token, T body, string culture = "en",
        string? pathParameter = null)
    {
        AuthorizeRequest(token);
        SetRequestCulture(culture);

        if (!string.IsNullOrWhiteSpace(pathParameter))
            requestUri = $"{requestUri}/{pathParameter}";

        return await _httpClient.PutAsJsonAsync(requestUri, body);
    }

    protected async Task<HttpResponseMessage> DoDelete(string requestUri, string? token, string culture = "en",
        string? pathParameter = null)
    {
        AuthorizeRequest(token);
        SetRequestCulture(culture);

        if (!string.IsNullOrWhiteSpace(pathParameter))
            requestUri = $"{requestUri}/{pathParameter}";

        return await _httpClient.DeleteAsync(requestUri);
    }

    private void AuthorizeRequest(string? token)
    {
        if (!string.IsNullOrWhiteSpace(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void SetRequestCulture(string? culture)
    {
        if (string.IsNullOrWhiteSpace(culture)) return;
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}