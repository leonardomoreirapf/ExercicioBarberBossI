using AutoMapper;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Repositories.Faturamentos;

namespace BarberBossI.Application.UseCases.Faturamento.GetAll;

public class GetAllFaturamentosUseCase : IGetAllFaturamentosUseCase
{
	private IFaturamentoReadOnlyRepository _repository;
	private IMapper _mapper;
	public GetAllFaturamentosUseCase(IFaturamentoReadOnlyRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<ResponseFaturamentosJson> Execute()
	{
		var result = await _repository.GetAll();

		return new ResponseFaturamentosJson
		{
			Faturamentos = _mapper.Map<List<ResponseShortFaturamentoJson>>(result)
		};
	}
}
