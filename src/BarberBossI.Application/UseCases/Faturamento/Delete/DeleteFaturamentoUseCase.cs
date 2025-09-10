using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Domain.Services.LoggedUser;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.Faturamento.Delete;

public class DeleteFaturamentoUseCase : IDeleteFaturamentoUseCase
{
	private IFaturamentoReadOnlyRepository _faturamentoReadOnly;
	private IFaturamentoWriteOnlyRepository _repository;
	private IUnitOfWork _unitOfWork;
	private readonly ILoggedUser _loggedUser;

	public DeleteFaturamentoUseCase(
		IFaturamentoReadOnlyRepository faturamentoReadOnly,
		IFaturamentoWriteOnlyRepository repository, 
		IUnitOfWork unitOfWork,
		ILoggedUser loggedUser)
	{
		_faturamentoReadOnly = faturamentoReadOnly;
		_repository = repository;
		_unitOfWork = unitOfWork;
		_loggedUser = loggedUser;
	}
	public async Task Execute(long id)
	{
		var loggedUser = await _loggedUser.Get();

		var faturamento = await _faturamentoReadOnly.GetById(loggedUser, id);

		if (faturamento is null)
			throw new NotFoundException(ResourceErrorMessage.FaturamentoNaoEncontrado);
		
		await _repository.Delete(id);

		await _unitOfWork.Commit();
	}
}
