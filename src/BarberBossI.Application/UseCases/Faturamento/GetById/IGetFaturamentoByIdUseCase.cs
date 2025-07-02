using BarberBossI.Communication.Responses;

namespace BarberBossI.Application.UseCases.Faturamento.GetById
{
	public interface IGetFaturamentoByIdUseCase
	{
		Task<ResponseFaturamentoJson> Execute(long id);
	}
}
