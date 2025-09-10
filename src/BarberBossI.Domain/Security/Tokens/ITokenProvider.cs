namespace BarberBossI.Domain.Security.Tokens;

public interface ITokenProvider
{
	string TokenOnRequest();
}
