using GigAuth.Communication.Responses;
using GigAuth.Domain.Filters;

namespace GigAuth.Application.UseCases.Roles.GetFiltered;

public interface IGetFilteredRolesUseCase
{
    Task<List<ResponseRole>?> Execute(RequestRoleFilter filter);
}