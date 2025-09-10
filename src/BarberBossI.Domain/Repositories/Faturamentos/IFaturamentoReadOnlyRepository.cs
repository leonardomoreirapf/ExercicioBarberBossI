using BarberBossI.Domain.Entities;

namespace BarberBossI.Domain.Repositories.Faturamentos;

public interface IFaturamentoReadOnlyRepository
{
	Task<List<Faturamento>> GetAll(Entities.User user);
	Task<Faturamento?> GetById(Entities.User user, long id);
	Task<List<Faturamento>> FilterByMonth(Entities.User user, DateOnly date);
}
