namespace GigAuth.Application.UseCases.Auth.ForgotPassword;

public interface IForgotPasswordUseCase
{
    public Task Execute(string userName);
}