using GigAuth.Domain.Services.SupabaseProvider;
using Microsoft.Extensions.Configuration;

namespace GigAuth.Infrastructure.Services.SupabaseProvider;

public class SupabaseClientFactory(IConfiguration configuration) : ISupabaseClientFactory
{
    public async Task<Supabase.Client> CreateClient()
    {
        var url = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? configuration["Supabase:Url"];
        var key = Environment.GetEnvironmentVariable("SUPABASE_KEY") ??  configuration["Supabase:Key"];

        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true
        };

        var supabase = new Supabase.Client(url!, key!, options);
        await supabase.InitializeAsync();

        return supabase;
    }
}