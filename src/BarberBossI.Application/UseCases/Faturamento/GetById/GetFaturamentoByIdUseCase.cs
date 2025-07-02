using AutoMapper;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.Faturamento.GetById;

public class GetFaturamentoByIdUseCase : IGetFaturamentoByIdUseCase
{
	private IFaturamentoReadOnlyRepository _repository;
	private IMapper _mapper;

	public GetFaturamentoByIdUseCase(IFaturamentoReadOnlyRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<ResponseFaturamentoJson> Execute(long id)
	{
		var result = await _repository.GetById(id);

		if (result is null)
			throw new NotFoundException(ResourceErrorMessage.FaturamentoNaoEncontrado);

		return _mapper.Map<ResponseFaturamentoJson>(result);
	}
}
