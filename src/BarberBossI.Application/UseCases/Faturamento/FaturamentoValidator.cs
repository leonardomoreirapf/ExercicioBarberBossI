using BarberBossI.Communication.Requests;
using BarberBossI.Exception;
using FluentValidation;

namespace BarberBossI.Application.UseCases.Faturamento;

public class FaturamentoValidator : AbstractValidator<RequestFaturamentoJson>
{
	public FaturamentoValidator()
	{
		RuleFor(faturamento => faturamento.Titulo).NotEmpty().WithMessage(ResourceErrorMessage.TituloObrigatorio);
		RuleFor(faturamento => faturamento.Valor).GreaterThan(0).WithMessage(ResourceErrorMessage.ValorMaiorQueZero);
		RuleFor(faturamento => faturamento.Data).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessage.DataMaiorDataAtual);
		RuleFor(faturamento => faturamento.TipoPagamento).IsInEnum().WithMessage(ResourceErrorMessage.TipoPagamentoInvalido);
		RuleFor(faturamento => faturamento.Descricao).NotEmpty().WithMessage(ResourceErrorMessage.DescricaoObrigatoria);
	}
}
