using BarberBossI.Domain.Repositories.Faturamentos;
using Moq;

namespace CommonTesteUtilities.Repositories;

public class FaturamentoWriteOnlyRepositoryBuilder
{
	public static IFaturamentoWriteOnlyRepository Build()
	{
		var mock = new Mock<IFaturamentoWriteOnlyRepository>();

		return mock.Object;
	}
}
