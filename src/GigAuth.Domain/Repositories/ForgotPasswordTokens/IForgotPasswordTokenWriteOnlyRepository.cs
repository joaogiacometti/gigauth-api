using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.ForgotPasswordTokens;

public interface IForgotPasswordTokenWriteOnlyRepository
{
    Task Create(ForgotPasswordToken token);
    void Update(ForgotPasswordToken token);
}