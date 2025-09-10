using BarberBossI.Domain.Entities;

namespace BarberBossI.Domain.Repositories.Faturamentos;

public interface IFaturamentoUpdateOnlyRepository
{
	Task<Faturamento?> GetById(long userId, long id);
	void Update(Faturamento faturamento);
}
