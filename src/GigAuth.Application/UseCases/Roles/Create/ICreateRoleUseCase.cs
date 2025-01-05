using GigAuth.Communication.Requests;

namespace GigAuth.Application.UseCases.Roles.Create;

public interface ICreateRoleUseCase
{
    Task Execute(RequestCreateRole request);
}