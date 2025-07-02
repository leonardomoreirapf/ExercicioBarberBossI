using BarberBossI.Domain.Entities;

namespace BarberBossI.Domain.Repositories.Faturamentos;

public interface IFaturamentoWriteOnlyRepository
{
	Task Add(Faturamento faturamento);
	Task<bool> Delete(long id);
}
