using BarberBossI.Application.UseCases.Faturamento.Register;
using BarberBossI.Domain.Entities;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using CommonTesteUtilities;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Mapper;
using CommonTesteUtilities.Repositories;
using CommonTesteUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Faturamento.Register;

public class RegisterFaturamentoUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var loggedUser = UserBuilder.Build();
		var request = RequestFaturamentoJsonBuilder.Build();
		var useCase = CreateUseCase(loggedUser);

		var result = await useCase.Execute(request);

		result.Should().NotBeNull();
		result.Titulo.Should().Be(request.Titulo);
	}

	[Fact]
	public async Task Error_Title_Empty()
	{
		var loggedUser = UserBuilder.Build();
		var request = RequestFaturamentoJsonBuilder.Build();
		request.Titulo = string.Empty;

		var useCase = CreateUseCase(loggedUser);

		var act = async () => await useCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(exception => exception.GetErrors().Count().Equals(1) && exception.GetErrors().Contains(ResourceErrorMessage.TituloObrigatorio));
	}

	private RegisterFaturamentoUseCase CreateUseCase(User user)
	{
		var repository = FaturamentoWriteOnlyRepositoryBuilder.Build();
		var mapper = MapperBuilder.Build();
		var unitOfWork = UnitOfWorkBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new RegisterFaturamentoUseCase(repository, unitOfWork, mapper, loggedUser);
	}
}
