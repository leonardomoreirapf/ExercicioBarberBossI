using BarberBossI.Communication.Requests;

namespace BarberBossI.Application.UseCases.Faturamento.Update;

public interface IUpdateFaturamentoUseCase
{
	Task Execute(long id, RequestFaturamentoJson request);
}
