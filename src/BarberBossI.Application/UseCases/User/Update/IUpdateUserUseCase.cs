using BarberBossI.Communication.Requests;

namespace BarberBossI.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
	Task Execute(RequestUpdateUserJson request);
}
