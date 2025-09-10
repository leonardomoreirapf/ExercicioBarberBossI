using BarberBossI.Communication.Requests;

namespace BarberBossI.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
	Task Execute(RequestChangePasswordJson request);
}
