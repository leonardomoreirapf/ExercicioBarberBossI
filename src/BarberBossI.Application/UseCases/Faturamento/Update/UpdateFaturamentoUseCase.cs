using AutoMapper;
using BarberBossI.Communication.Requests;
using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.Faturamento.Update;

public class UpdateFaturamentoUseCase : IUpdateFaturamentoUseCase
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFaturamentoUpdateOnlyRepository _repository;
	public UpdateFaturamentoUseCase(IMapper mapper, IUnitOfWork unitOfWork, IFaturamentoUpdateOnlyRepository repository)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_repository = repository;
	}
	public async Task Execute(long id, RequestFaturamentoJson request)
	{
		Validate(request);

		var faturamento = await _repository.GetById(id);

		if (faturamento is null)
			throw new NotFoundException(ResourceErrorMessage.FaturamentoNaoEncontrado);

		_mapper.Map(request, faturamento);

		_repository.Update(faturamento);

		await _unitOfWork.Commit();
	}

	private void Validate(RequestFaturamentoJson request)
	{
		var validator = new FaturamentoValidator();

		var result = validator.Validate(request);

		if (result.IsValid)
			return;

		var errorMessages = result.Errors.Select(erro => erro.ErrorMessage).ToList();

		throw new ErrorOnValidationException(errorMessages.ToList());
	}
}
