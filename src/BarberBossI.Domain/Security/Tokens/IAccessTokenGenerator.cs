using BarberBossI.Domain.Entities;

namespace BarberBossI.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
	string Generate(User user);
}
