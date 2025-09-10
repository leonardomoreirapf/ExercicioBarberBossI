using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Services.LoggedUser;
using Moq;

namespace CommonTesteUtilities;

public class LoggedUserBuilder
{
	public static ILoggedUser Build(User user)
	{
		var mock = new Mock<ILoggedUser>();

		mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

		return mock.Object;
	}
}
