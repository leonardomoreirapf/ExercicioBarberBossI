using BarberBossI.Communication.Enums;

namespace BarberBossI.Communication.Requests;

public class RequestFaturamentoJson
{
	public string Titulo { get; set; } = string.Empty;
	public DateTime Data { get; set; }
	public TipoPagamento TipoPagamento { get; set; }
	public decimal Valor { get; set; }
	public string Descricao { get; set; } = string.Empty;
}
