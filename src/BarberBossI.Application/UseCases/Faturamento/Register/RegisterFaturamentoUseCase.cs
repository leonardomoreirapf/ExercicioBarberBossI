using AutoMapper;
using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Domain.Services.LoggedUser;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.Faturamento.Register;

public class RegisterFaturamentoUseCase : IRegisterFaturamentoUseCase
{
	private readonly IFaturamentoWriteOnlyRepository _repository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ILoggedUser _loggedUser;
	public RegisterFaturamentoUseCase(
		IFaturamentoWriteOnlyRepository repository, 
		IUnitOfWork unitOfWork, 
		IMapper mapper,
		ILoggedUser loggedUser)
	{
		_repository = repository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_loggedUser = loggedUser;
	}
	public async Task<ResponseRegisteredFaturamentoJson> Execute(RequestFaturamentoJson request)
	{
		Validate(request);

		var loggedUser = await _loggedUser.Get();

		var faturamento = _mapper.Map<Domain.Entities.Faturamento>(request);
		faturamento.UserId = loggedUser.Id;

		await _repository.Add(faturamento);
		await _unitOfWork.Commit();

		return _mapper.Map<ResponseRegisteredFaturamentoJson>(faturamento);
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
