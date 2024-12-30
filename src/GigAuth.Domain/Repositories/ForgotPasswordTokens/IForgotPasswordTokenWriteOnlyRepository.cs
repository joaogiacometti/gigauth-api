using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.ForgotPasswordTokens;

public interface IForgotPasswordTokenWriteOnlyRepository
{
    Task<ForgotPasswordToken?> GetByToken(string token);
    Task Create(ForgotPasswordToken token);
    void Delete(ForgotPasswordToken token);
}