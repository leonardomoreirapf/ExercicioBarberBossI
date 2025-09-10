using BarberBossI.Application.UseCases.Login;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using CommonTesteUtilities.Cryptography;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Repositories;
using CommonTesteUtilities.Requests;
using CommonTesteUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var user = UserBuilder.Build();

		var request = RequestLoginJsonBuilder.Build();
		request.Email = user.Email;

		var useCase = CreateUseCase(user, request.Password);

		var result = await useCase.Execute(request);

		result.Should().NotBeNull();
		result.Name.Should().Be(user.Name);
		result.Token.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Error_User_Not_Found()
	{
		var user = UserBuilder.Build();
		var request = RequestLoginJsonBuilder.Build();

		var useCase = CreateUseCase(user, request.Password);

		var act = async () => await useCase.Execute(request);

		var result = await act.Should().ThrowAsync<InvalidLoginException>();

		result.Where(ex => ex.GetErrors().Count.Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.LoginOuSenhaInvalido));
	}

	[Fact]
	public async Task Error_Password_Not_Math()
	{
		var user = UserBuilder.Build();
		var request = RequestLoginJsonBuilder.Build();
		request.Email = user.Email;

		var useCase = CreateUseCase(user);

		var act = async () => await useCase.Execute(request);

		var result = await act.Should().ThrowAsync<InvalidLoginException>();

		result.Where(ex => ex.GetErrors().Count.Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.LoginOuSenhaInvalido));
	}

	private DoLoginUseCase CreateUseCase(BarberBossI.Domain.Entities.User user, string? password = null)
	{
		var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
		var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();
		var tokenGenerator = JwtTokenGeneratorBuilder.Build();

		return new DoLoginUseCase(readRepository, passwordEncripter, tokenGenerator);
	}
}
