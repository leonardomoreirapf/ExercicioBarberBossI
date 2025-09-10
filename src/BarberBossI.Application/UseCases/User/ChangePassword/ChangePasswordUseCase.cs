using BarberBossI.Communication.Requests;
using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.User;
using BarberBossI.Domain.Security.Cryptography;
using BarberBossI.Domain.Services.LoggedUser;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BarberBossI.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
	private readonly ILoggedUser _loggedUser;
	private readonly IUserUpdateOnlyRepository _repository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPasswordEncripter _passwordEncripter;

	public ChangePasswordUseCase(
		ILoggedUser loggedUser,
		IUserUpdateOnlyRepository repository,
		IUnitOfWork unitOfWork,
		IPasswordEncripter passwordEncripter)
	{
		_loggedUser = loggedUser;
		_repository = repository;
		_unitOfWork = unitOfWork;
		_passwordEncripter = passwordEncripter;
	}

	public async Task Execute(RequestChangePasswordJson request)
	{
		var loggedUser = await _loggedUser.Get();

		Validate(request, loggedUser);

		var user = await _repository.GetById(loggedUser.Id);
		user.Password = _passwordEncripter.Encrypt(request.NewPassword);

		_repository.Update(user);

		await _unitOfWork.Commit();
	}

	private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
	{
		var validator = new ChangePasswordValidator();

		var result = validator.Validate(request);

		var passwordMatch = _passwordEncripter.Verify(request.Password, loggedUser.Password);

		if (!passwordMatch)
			result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessage.SenhaDiferenteDaAtual));

		if (result.IsValid)
			return;

		var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

		throw new ErrorOnValidationException(errors);
	}
}
