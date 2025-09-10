using BarberBossI.Application.UseCases.User.Register;
using BarberBossI.Application.UseCases.User.Update;
using BarberBossI.Domain.Entities;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using CommonTesteUtilities;
using CommonTesteUtilities.Cryptography;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Mapper;
using CommonTesteUtilities.Repositories;
using CommonTesteUtilities.Requests;
using CommonTesteUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Update;

public class UpdateUserUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var user = UserBuilder.Build();
		var request = RequestUpdateUserJsonBuilder.Build();

		var userCase = CreateUseCase(user);

		var act = async () => await userCase.Execute(request);

		await act.Should().NotThrowAsync();

		user.Name.Should().Be(request.Name);
		user.Email.Should().Be(request.Email);
	}

	[Fact]
	public async Task Error_Name_Empty()
	{
		var user = UserBuilder.Build();

		var request = RequestUpdateUserJsonBuilder.Build();
		request.Name = string.Empty;

		var userCase = CreateUseCase(user);

		var act = async () => await userCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count().Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.NomeVazio));
	}

	[Fact]
	public async Task Error_Email_Already_Exist()
	{
		var user = UserBuilder.Build();

		var request = RequestUpdateUserJsonBuilder.Build();

		var userCase = CreateUseCase(user, request.Email);

		var act = async () => await userCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count().Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.EmailExistente));
	}

	private UpdateUserUseCase CreateUseCase(User user, string? email = null)
	{
		var unitOfWork = UnitOfWorkBuilder.Build();
		var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
		var loggedUser = LoggedUserBuilder.Build(user);
		var readRepository = new UserReadOnlyRepositoryBuilder();

		if (!string.IsNullOrWhiteSpace(email))
		{
			readRepository.ExistActiveUserWithEmail(email);
		}


		return new UpdateUserUseCase(loggedUser, updateRepository, readRepository.Build(), unitOfWork);
	}
}
