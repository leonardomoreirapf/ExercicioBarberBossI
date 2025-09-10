using AutoMapper;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Domain.Services.LoggedUser;

namespace BarberBossI.Application.UseCases.Faturamento.GetAll;

public class GetAllFaturamentosUseCase : IGetAllFaturamentosUseCase
{
	private IFaturamentoReadOnlyRepository _repository;
	private IMapper _mapper;
	private readonly ILoggedUser _loggedUser;
	public GetAllFaturamentosUseCase(
		IFaturamentoReadOnlyRepository repository, 
		IMapper mapper,
		ILoggedUser loggedUser)
	{
		_repository = repository;
		_mapper = mapper;
		_loggedUser = loggedUser;
	}

	public async Task<ResponseFaturamentosJson> Execute()
	{
		var loggedUser = await _loggedUser.Get();

		var result = await _repository.GetAll(loggedUser);

		return new ResponseFaturamentosJson
		{
			Faturamentos = _mapper.Map<List<ResponseShortFaturamentoJson>>(result)
		};
	}
}
