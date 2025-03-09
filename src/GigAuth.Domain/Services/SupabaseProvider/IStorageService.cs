namespace GigAuth.Domain.Services.SupabaseProvider;

public interface IStorageService
{
    Task<string?> UploadAvatar(byte[] base64, string fileName);
}