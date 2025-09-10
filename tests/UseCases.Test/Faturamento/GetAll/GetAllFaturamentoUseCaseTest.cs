using BarberBossI.Application.UseCases.Faturamento.GetAll;
using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Repositories.Faturamentos;
using CommonTesteUtilities;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Mapper;
using CommonTesteUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Faturamento.GetAll;

public class GetAllFaturamentoUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var loggedUser = UserBuilder.Build();
		var faturamentos = FaturamentoBuilder.Collection(loggedUser);

		var useCase = CreateUseCase(loggedUser, faturamentos);

		var result = await useCase.Execute();

		result.Should().NotBeNull();
		result.Faturamentos.Should().NotBeNullOrEmpty().And.AllSatisfy(faturamento =>
		{
			faturamento.Id.Should().BeGreaterThan(0);
			faturamento.Titulo.Should().NotBeNullOrEmpty();
			faturamento.Valor.Should().BeGreaterThan(0);
		});
	}

	private GetAllFaturamentosUseCase CreateUseCase(User user, List<BarberBossI.Domain.Entities.Faturamento> faturamentos)
	{
		var repository = new FaturamentoReadOnlyRepositoryBuilder().GetAll(user, faturamentos).Build();
		var mapper = MapperBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new GetAllFaturamentosUseCase(repository, mapper, loggedUser);
	}
}
