using BarberBossI.Domain.Entities;

namespace BarberBossI.Domain.Repositories.Faturamentos;

public interface IFaturamentoReadOnlyRepository
{
	Task<List<Faturamento>> GetAll();
	Task<Faturamento?> GetById(long id);
	Task<List<Faturamento>> FilterByMonth(DateOnly date);
}
