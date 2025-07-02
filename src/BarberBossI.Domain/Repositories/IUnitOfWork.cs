namespace BarberBossI.Domain.Repositories;

public interface IUnitOfWork
{
	Task Commit();
}
