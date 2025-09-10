using BarberBossI.Domain.Entities;

namespace BarberBossI.Domain.Repositories.Faturamentos;

public interface IFaturamentoWriteOnlyRepository
{
	Task Add(Faturamento faturamento);
	Task Delete(long id);
}
