using GigAuth.Domain.Repositories;
using GigAuth.Domain.Repositories.Users;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class UnitOfWorkBuilder
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    
    public IUnitOfWork Build() => _unitOfWork.Object;
}