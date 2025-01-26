using System.Text.Json;
using FluentAssertions;

namespace CommonTestsUtilities.Extensions;

public static class HttpResponseMessageExtensions
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public static async Task<T?> Deserialize<T>(this HttpResponseMessage result)
    {
        var content = await result.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, Options);
    }
    
    public static async Task CompareException(this HttpResponseMessage result, string expectedMessage)
    {
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errorList = response.RootElement.GetProperty("ErrorMessages").EnumerateArray();

        errorList.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}