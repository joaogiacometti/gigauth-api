using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.ForgotPasswordTokens;

public interface IForgotPasswordTokenReadOnlyRepository
{
    Task<ForgotPasswordToken?> GetByUserId(Guid userId);
}