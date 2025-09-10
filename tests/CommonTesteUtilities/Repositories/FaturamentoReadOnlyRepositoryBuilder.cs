using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Repositories.Faturamentos;
using Moq;

namespace CommonTesteUtilities.Repositories;

public class FaturamentoReadOnlyRepositoryBuilder
{
	private readonly Mock<IFaturamentoReadOnlyRepository> _repository;

	public FaturamentoReadOnlyRepositoryBuilder()
	{
		_repository = new Mock<IFaturamentoReadOnlyRepository>();
	}
	public FaturamentoReadOnlyRepositoryBuilder GetAll(User user, List<Faturamento> faturamentos)
	{
		_repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(faturamentos);

		return this;
	}

	public FaturamentoReadOnlyRepositoryBuilder GetById(User user, Faturamento? faturamento)
	{
		if (faturamento is not null)
			_repository.Setup(repository => repository.GetById(user, faturamento.Id)).ReturnsAsync(faturamento);

		return this;
	}

	public FaturamentoReadOnlyRepositoryBuilder FilterByMonth(User user, List<Faturamento> faturamentos)
	{
		_repository.Setup(repository => repository.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(faturamentos);

		return this;
	}

	public IFaturamentoReadOnlyRepository Build() => _repository.Object;
}
