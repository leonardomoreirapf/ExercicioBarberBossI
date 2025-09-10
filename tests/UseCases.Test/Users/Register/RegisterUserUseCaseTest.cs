using BarberBossI.Application.UseCases.User.Register;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using CommonTesteUtilities.Cryptography;
using CommonTesteUtilities.Mapper;
using CommonTesteUtilities.Repositories;
using CommonTesteUtilities.Requests;
using CommonTesteUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var request = RequestRegisterUserJsonBuilder.Build();
		var userCase = CreateUseCase();

		var result = await userCase.Execute(request);

		result.Should().NotBeNull();
		result.Name.Should().Be(result.Name);
		result.Token.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Error_Name_Empty()
	{
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Name = string.Empty;

		var userCase = CreateUseCase();

		var act = async () => await userCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count().Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.NomeVazio));
	}

	[Fact]
	public async Task Error_Email_Already_Exist()
	{
		var request = RequestRegisterUserJsonBuilder.Build();

		var userCase = CreateUseCase(request.Email);

		var act = async () => await userCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count().Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.EmailExistente));
	}

	private RegisterUserUseCase CreateUseCase(string? email = null)
	{
		var mapper = MapperBuilder.Build();
		var unitOfWork = UnitOfWorkBuilder.Build();
		var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
		var readRepository = new UserReadOnlyRepositoryBuilder();
		var passwordEncripter = new PasswordEncripterBuilder().Build();
		var tokenGenerator = JwtTokenGeneratorBuilder.Build();

		if (!string.IsNullOrWhiteSpace(email))
		{
			readRepository.ExistActiveUserWithEmail(email);
		}


		return new RegisterUserUseCase(mapper, passwordEncripter, readRepository.Build(), writeRepository, unitOfWork, tokenGenerator);
	}
}
