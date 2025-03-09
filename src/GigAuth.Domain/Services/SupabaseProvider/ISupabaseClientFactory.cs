namespace GigAuth.Domain.Services.SupabaseProvider;

public interface ISupabaseClientFactory
{
    Task<Supabase.Client> CreateClient();
}