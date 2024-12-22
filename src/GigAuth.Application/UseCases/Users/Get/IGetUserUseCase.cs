using GigAuth.Communication.Responses;
using GigAuth.Domain.Entities;

namespace GigAuth.Application.UseCases.Users.Get;

public interface IGetUserUseCase
{
    Task<ResponseUserShort> Execute(Guid id);
}