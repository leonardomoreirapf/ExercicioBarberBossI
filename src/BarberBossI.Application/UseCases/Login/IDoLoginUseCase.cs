using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;

namespace BarberBossI.Application.UseCases.Login;

public interface IDoLoginUseCase
{
	Task<ResponseRegisterUserJson> Execute(RequestLoginJson request);
}
