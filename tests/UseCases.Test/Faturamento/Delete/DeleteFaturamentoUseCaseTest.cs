using BarberBossI.Application.UseCases.Faturamento.Delete;
using BarberBossI.Domain.Entities;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using CommonTesteUtilities;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Repositories;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Test.Faturamento.Delete;

public class DeleteFaturamentoUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var loggedUser = UserBuilder.Build();
		var faturamento = FaturamentoBuilder.Build(loggedUser);

		var useCase = CreateUseCase(loggedUser, faturamento);

		var act = async () => await useCase.Execute(faturamento.Id);

		await act.Should().NotThrowAsync();
	}

	[Fact]
	public async Task Error_Faturamento_Not_Found()
	{
		var loggedUser = UserBuilder.Build();

		var useCase = CreateUseCase(loggedUser);

		var act = async () => await useCase.Execute(id: 1000);

		var result = await act.Should().ThrowAsync<NotFoundException>();

		result.Where(ex => ex.GetErrors().Count.Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.FaturamentoNaoEncontrado));
	}

	private DeleteFaturamentoUseCase CreateUseCase(User user, BarberBossI.Domain.Entities.Faturamento? faturamento = null)
	{
		var repositoryWriteOnly = FaturamentoWriteOnlyRepositoryBuilder.Build();
		var repository = new FaturamentoReadOnlyRepositoryBuilder().GetById(user, faturamento).Build();
		var unitOfWork = UnitOfWorkBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new DeleteFaturamentoUseCase(repository, repositoryWriteOnly, unitOfWork, loggedUser);
	}
}
