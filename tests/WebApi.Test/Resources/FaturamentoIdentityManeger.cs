using BarberBossI.Domain.Entities;

namespace WebApi.Test.Resources;

public class FaturamentoIdentityManeger
{
	private readonly Faturamento _faturamento;

	public FaturamentoIdentityManeger(Faturamento faturamento)
	{
		_faturamento = faturamento;
	}

	public long GetFaturamentoId() => _faturamento.Id;
	public DateTime GetDate() => _faturamento.Data;
}
