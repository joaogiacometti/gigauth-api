using GigAuth.Domain.Services.SupabaseProvider;
using FileOptions = Supabase.Storage.FileOptions;

namespace GigAuth.Infrastructure.Services.SupabaseProvider;

public class StorageService(ISupabaseClientFactory supabaseClientFactory) : IStorageService
{
    public async Task<string?> UploadAvatar(byte[] base64, string fileName)
    {
        var supabase = await supabaseClientFactory.CreateClient();

        var extension = Path.GetExtension(fileName);
        var filePath = $"public/avatars/{Guid.NewGuid()}{extension}";
        
        var options = new FileOptions() { CacheControl = "3600", Upsert = false };
            
        await supabase.Storage
            .From("gigauth")
            .Upload(base64, filePath, options);
            
        var publicUrl = supabase.Storage
            .From("gigauth")
            .GetPublicUrl(filePath);

        return publicUrl;
    }
}