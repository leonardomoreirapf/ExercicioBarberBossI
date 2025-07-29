using BarberBossI.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace BarberBossI.Infrastructure.Security.Cryptography;

public class BCrypt : IPasswordEncripter
{
	public string Encrypt(string password)
	{
		var passwordHash = BC.HashPassword(password);

		return passwordHash;
	}

	public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);

}
