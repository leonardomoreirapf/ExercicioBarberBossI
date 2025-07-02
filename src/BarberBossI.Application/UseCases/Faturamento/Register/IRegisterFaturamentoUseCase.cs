using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;

namespace BarberBossI.Application.UseCases.Faturamento.Register
{
	public interface IRegisterFaturamentoUseCase
	{
		Task<ResponseRegisteredFaturamentoJson> Execute(RequestFaturamentoJson request);
	}
}
