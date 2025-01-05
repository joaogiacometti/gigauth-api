using GigAuth.Communication.Responses;

namespace GigAuth.Application.UseCases.Roles.Get;

public interface IGetRoleUseCase
{
    Task<ResponseRole> Execute(Guid id);
}