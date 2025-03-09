using GigAuth.Domain.Services.SupabaseProvider;
using Moq;

namespace CommonTestsUtilities.Services.SupabaseProvider;

public class StorageServiceBuilder
{
    private readonly Mock<IStorageService> _service = new();

    public IStorageService Build() => _service.Object;
}