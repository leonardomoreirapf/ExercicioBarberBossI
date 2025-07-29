using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;

namespace BarberBossI.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
	Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
}
