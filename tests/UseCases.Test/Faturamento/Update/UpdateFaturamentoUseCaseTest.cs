using BarberBossI.Application.UseCases.Faturamento.Update;
using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;
using CommonTesteUtilities;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Mapper;
using CommonTesteUtilities.Repositories;
using CommonTesteUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Faturamento.Update;

public class UpdateFaturamentoUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var loggedUser = UserBuilder.Build();
		var request = RequestFaturamentoJsonBuilder.Build();
		var faturamento = FaturamentoBuilder.Build(loggedUser);

		var useCase = CreateUseCase(loggedUser, faturamento);

		var act = async () => await useCase.Execute(id: faturamento.Id, request);

		await act.Should().NotThrowAsync();

		faturamento.Titulo.Should().Be(request.Titulo);
		faturamento.Descricao.Should().Be(request.Descricao);
		faturamento.Data.Should().Be(request.Data);
		faturamento.Valor.Should().Be(request.Valor);
		faturamento.TipoPagamento.Should().Be((BarberBossI.Domain.Enums.TipoPagamento)request.TipoPagamento);
	}

	[Fact]
	public async Task Error_Title_Empty()
	{
		var loggedUser = UserBuilder.Build();
		var faturamento = FaturamentoBuilder.Build(loggedUser);

		var request = RequestFaturamentoJsonBuilder.Build();
		request.Titulo = string.Empty;

		var useCase = CreateUseCase(loggedUser, faturamento);

		var act = async () => await useCase.Execute(id: faturamento.Id, request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count.Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.TituloObrigatorio));
	}

	[Fact]
	public async Task Error_Faturamento_Not_Found()
	{
		var loggedUser = UserBuilder.Build();

		var request = RequestFaturamentoJsonBuilder.Build();

		var useCase = CreateUseCase(loggedUser);

		var act = async () => await useCase.Execute(id: 1000, request);

		var result = await act.Should().ThrowAsync<NotFoundException>();

		result.Where(ex => ex.GetErrors().Count.Equals(1) && ex.GetErrors().Contains(ResourceErrorMessage.FaturamentoNaoEncontrado));
	}

	private UpdateFaturamentoUseCase CreateUseCase(User user, BarberBossI.Domain.Entities.Faturamento? faturamento = null) 
	{
		var repository = new FaturamentoUpdateOnlyRepositoryBuilder().GetById(user, faturamento).Build();
		var mapper = MapperBuilder.Build();
		var unitOfWork = UnitOfWorkBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new UpdateFaturamentoUseCase(mapper, unitOfWork, repository, loggedUser);
	}

}
