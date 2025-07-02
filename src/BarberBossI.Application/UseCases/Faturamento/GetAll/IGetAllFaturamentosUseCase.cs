using BarberBossI.Communication.Responses;

namespace BarberBossI.Application.UseCases.Faturamento.GetAll;

public interface IGetAllFaturamentosUseCase
{
	Task<ResponseFaturamentosJson> Execute();
}
