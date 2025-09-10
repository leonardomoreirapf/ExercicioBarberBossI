using BarberBossI.Application.UseCases.Faturamento.GetById;
using BarberBossI.Domain.Entities;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using CommonTesteUtilities;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Mapper;
using CommonTesteUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Faturamento.GetById;

public class GetFaturamentoByIdUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var loggedUser = UserBuilder.Build();
		var faturamento = FaturamentoBuilder.Build(loggedUser);

		var useCase = CreateUseCase(loggedUser, faturamento);

		var result = await useCase.Execute(faturamento.Id);

		result.Should().NotBeNull();
		result.Id.Should().Be(faturamento.Id);
		result.Titulo.Should().Be(faturamento.Titulo);
		result.Descricao.Should().Be(faturamento.Descricao);
		result.Data.Should().Be(faturamento.Data);
		result.Valor.Should().Be(faturamento.Valor);
		result.TipoPagamento.Should().Be((BarberBossI.Communication.Enums.TipoPagamento)faturamento.TipoPagamento);
	}

	[Fact]
	public async Task Error_Faturamento_Not_Found()
	{
		var loggedUser = UserBuilder.Build();

		var useCase = CreateUseCase(loggedUser);

		var act = async () => await useCase.Execute(id: 1000);

		var result = await act.Should().ThrowAsync<NotFoundException>();

		result.Where(exception => exception.GetErrors().Count.Equals(1) && exception.GetErrors().Contains(ResourceErrorMessage.FaturamentoNaoEncontrado));
	}

	private GetFaturamentoByIdUseCase CreateUseCase(User user, BarberBossI.Domain.Entities.Faturamento? faturamento = null)
	{
		var repository = new FaturamentoReadOnlyRepositoryBuilder().GetById(user, faturamento).Build();
		var mapper = MapperBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new GetFaturamentoByIdUseCase(repository, mapper, loggedUser);
	}
}
