using AutoMapper;
using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.Faturamento.Register;

public class RegisterFaturamentoUseCase : IRegisterFaturamentoUseCase
{
	private readonly IFaturamentoWriteOnlyRepository _repository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	public RegisterFaturamentoUseCase(IFaturamentoWriteOnlyRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_repository = repository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}
	public async Task<ResponseRegisteredFaturamentoJson> Execute(RequestFaturamentoJson request)
	{
		Validate(request);

		var entity = _mapper.Map<Domain.Entities.Faturamento>(request);

		await _repository.Add(entity);
		await _unitOfWork.Commit();

		return _mapper.Map<ResponseRegisteredFaturamentoJson>(entity);
	}
	private void Validate(RequestFaturamentoJson request)
	{
		var validator = new FaturamentoValidator();
		var result = validator.Validate(request);

		if (result.IsValid)
			return;

		var errorMessages = result.Errors.Select(erro => erro.ErrorMessage);

		throw new ErrorOnValidationException(errorMessages.ToList());
	}
}
