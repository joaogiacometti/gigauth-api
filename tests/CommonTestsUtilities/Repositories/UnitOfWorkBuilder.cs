using GigAuth.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class UnitOfWorkBuilder
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public IUnitOfWork Build()
    {
        return _unitOfWork.Object;
    }
}