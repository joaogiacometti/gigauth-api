using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Roles.Update;

public interface IUpdateRoleUseCase
{
    Task Execute(Guid id, RequestRole request);
}