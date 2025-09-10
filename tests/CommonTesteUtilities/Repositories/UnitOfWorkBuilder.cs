using BarberBossI.Domain.Repositories;
using Moq;

namespace CommonTesteUtilities.Repositories;

public class UnitOfWorkBuilder
{
	public static IUnitOfWork Build()
	{
		var mock = new Mock<IUnitOfWork>();

		return mock.Object;
	}
}
