using System.Globalization;

namespace GigAuth.Api.Middlewares;

public class CultureMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    private static readonly HashSet<string> SupportedCultures =
    [
        ..CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(culture => culture.Name)
    ];

    public async Task Invoke(HttpContext context)
    {
        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();
        var cultureInfo = new CultureInfo("en");

        if (!string.IsNullOrEmpty(requestedCulture) && SupportedCultures.Contains(requestedCulture))
            cultureInfo = new CultureInfo(requestedCulture);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}