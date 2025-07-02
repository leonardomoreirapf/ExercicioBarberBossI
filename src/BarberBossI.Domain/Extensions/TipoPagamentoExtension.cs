using BarberBossI.Domain.Enums;

namespace BarberBossI.Domain.Extensions;

public static class TipoPagamentoExtension
{
	public static string TipoPagamentoToString(this TipoPagamento tipoPagamento)
	{
		return tipoPagamento switch
		{
			TipoPagamento.Dinheiro => "Dinheiro",
			TipoPagamento.CartaoCredito => "Cartão de crédito",
			TipoPagamento.CartaoDebito => "Cartão de débito",
			TipoPagamento.Pix => "Pix",
			_ => string.Empty
		};
	}
}
