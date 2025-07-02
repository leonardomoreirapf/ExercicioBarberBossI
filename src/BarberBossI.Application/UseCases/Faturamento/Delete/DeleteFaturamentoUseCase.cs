using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.Faturamento.Delete;

public class DeleteFaturamentoUseCase : IDeleteFaturamentoUseCase
{
	private IFaturamentoWriteOnlyRepository _repository;
	private IUnitOfWork _unitOfWork;

	public DeleteFaturamentoUseCase(IFaturamentoWriteOnlyRepository repository, IUnitOfWork unitOfWork)
	{
		_repository = repository;
		_unitOfWork = unitOfWork;
	}
	public async Task Execute(long id)
	{
		var result = await _repository.Delete(id);

		if (result is false)
			throw new NotFoundException(ResourceErrorMessage.FaturamentoNaoEncontrado);

		await _unitOfWork.Commit();
	}
}
