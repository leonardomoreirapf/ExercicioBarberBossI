using BarberBossI.Domain.Entities;

namespace BarberBossI.Domain.Repositories.Faturamentos;

public interface IFaturamentoUpdateOnlyRepository
{
	Task<Faturamento?> GetById(long id);
	void Update(Faturamento faturamento);
}
