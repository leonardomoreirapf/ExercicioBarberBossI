namespace BarberBossI.Communication.Responses;

public class ResponseShortFaturamentoJson
{
	public long Id { get; set; }
	public string Titulo { get; set; } = string.Empty;
	public decimal Valor { get; set; }
	public string Descricao { get; set; } = string.Empty;
}
