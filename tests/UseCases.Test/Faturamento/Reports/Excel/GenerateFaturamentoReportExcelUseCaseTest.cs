using BarberBossI.Application.UseCases.Faturamento.Reports.Excel;
using BarberBossI.Domain.Entities;
using CommonTesteUtilities;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Faturamento.Reports.Excel;

public class GenerateFaturamentoReportExcelUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var loggedUser = UserBuilder.Build();
		var faturamentos = FaturamentoBuilder.Collection(loggedUser);

		var useCase = CreateUseCase(loggedUser, faturamentos);

		var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Now));

		result.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Sucess_Empty()
	{
		var loggedUser = UserBuilder.Build();

		var useCase = CreateUseCase(loggedUser, new List<BarberBossI.Domain.Entities.Faturamento>());

		var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Now));

		result.Should().BeEmpty();
	}

	private GenerateFaturamentoReportExcelUseCase CreateUseCase(User user, List<BarberBossI.Domain.Entities.Faturamento> faturamentos)
	{
		var repository = new FaturamentoReadOnlyRepositoryBuilder().FilterByMonth(user, faturamentos).Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new GenerateFaturamentoReportExcelUseCase(repository, loggedUser);
	}
}
