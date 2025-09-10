using AutoMapper;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Domain.Services.LoggedUser;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.Faturamento.GetById;

public class GetFaturamentoByIdUseCase : IGetFaturamentoByIdUseCase
{
	private readonly IFaturamentoReadOnlyRepository _repository;
	private readonly IMapper _mapper;
	private readonly ILoggedUser _loggedUser;

	public GetFaturamentoByIdUseCase(
		IFaturamentoReadOnlyRepository repository, 
		IMapper mapper,
		ILoggedUser loggedUser)
	{
		_repository = repository;
		_mapper = mapper;
		_loggedUser = loggedUser;
	}

	public async Task<ResponseFaturamentoJson> Execute(long id)
	{
		var loggedUser = await _loggedUser.Get();

		var result = await _repository.GetById(loggedUser, id);

		if (result is null)
			throw new NotFoundException(ResourceErrorMessage.FaturamentoNaoEncontrado);

		return _mapper.Map<ResponseFaturamentoJson>(result);
	}
}
