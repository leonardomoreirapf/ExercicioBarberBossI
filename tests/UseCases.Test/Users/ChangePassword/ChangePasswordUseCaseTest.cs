using BarberBossI.Application.UseCases.User.ChangePassword;
using BarberBossI.Domain.Entities;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using CommonTesteUtilities;
using CommonTesteUtilities.Cryptography;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Repositories;
using CommonTesteUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.ChangePassword;

public class ChangePasswordUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var user = UserBuilder.Build();

		var request = RequestChangePasswordJsonBuilder.Build();

		var useCase = CreateUseCase(user, request.Password);

		var act = async () => await useCase.Execute(request);

		await act.Should().NotThrowAsync();
	}

	[Fact]
	public async Task Error_NewPassword_Empty()
	{
		var user = UserBuilder.Build();

		var request = RequestChangePasswordJsonBuilder.Build();
		request.NewPassword = string.Empty;

		var userCase = CreateUseCase(user, request.Password);

		var act = async () => await userCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count().Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.SenhaInvalida));
	}

	[Fact]
	public async Task Error_CurrentPassword_Different()
	{
		var user = UserBuilder.Build();

		var request = RequestChangePasswordJsonBuilder.Build();

		var userCase = CreateUseCase(user);

		var act = async () => await userCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count().Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.SenhaDiferenteDaAtual));
	}

	private static ChangePasswordUseCase CreateUseCase(User user, string?  password = null)
	{
		var unitOfWork = UnitOfWorkBuilder.Build();
		var userUpdateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
		var loggedUser = LoggedUserBuilder.Build(user);
		var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();

		return new ChangePasswordUseCase(loggedUser, userUpdateRepository, unitOfWork, passwordEncripter);
	}
}
