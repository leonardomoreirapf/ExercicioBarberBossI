using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Repositories.Faturamentos;
using Moq;

namespace CommonTesteUtilities.Repositories;

public class FaturamentoUpdateOnlyRepositoryBuilder
{
	private readonly Mock<IFaturamentoUpdateOnlyRepository> _repository;

	public FaturamentoUpdateOnlyRepositoryBuilder()
	{
		_repository = new Mock<IFaturamentoUpdateOnlyRepository>();
	}

	public FaturamentoUpdateOnlyRepositoryBuilder GetById(User user, Faturamento? faturamento)
	{
		if (faturamento is not null)
			_repository.Setup(repository => repository.GetById(user.Id, faturamento.Id)).ReturnsAsync(faturamento);

		return this;
	}

	public IFaturamentoUpdateOnlyRepository Build() => _repository.Object;
}
