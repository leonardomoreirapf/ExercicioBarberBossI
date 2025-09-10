using BarberBossI.Domain.Security.Cryptography;
using Moq;

namespace CommonTesteUtilities.Cryptography;

public class PasswordEncripterBuilder
{
	private readonly Mock<IPasswordEncripter> _mock;

	public PasswordEncripterBuilder()
	{
		_mock = new Mock<IPasswordEncripter>();

		_mock.Setup(config => config.Encrypt(It.IsAny<string>())).Returns("!asdaw123");
	}

	public PasswordEncripterBuilder Verify(string? password)
	{
		if(!string.IsNullOrWhiteSpace(password))
			_mock.Setup(config => config.Verify(password, It.IsAny<string>())).Returns(true);

		return this;
	}

	public IPasswordEncripter Build() => _mock.Object;
}
