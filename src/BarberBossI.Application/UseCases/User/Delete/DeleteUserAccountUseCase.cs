using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.User;
using BarberBossI.Domain.Services.LoggedUser;

namespace BarberBossI.Application.UseCases.User.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
	private readonly ILoggedUser _loggedUser;
	private readonly IUserWriteOnlyRepository _repository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteUserAccountUseCase(
		ILoggedUser loggedUser,
		IUserWriteOnlyRepository repository,
		IUnitOfWork unitOfWork)
	{
		_loggedUser = loggedUser;
		_repository = repository;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute()
	{
		var user = await _loggedUser.Get();

		await _repository.Delete(user);

		await _unitOfWork.Commit();
	}
}
