using BarberBossI.Domain.Entities;

namespace BarberBossI.Domain.Services.LoggedUser;

public interface ILoggedUser
{
	Task<User> Get();
}
