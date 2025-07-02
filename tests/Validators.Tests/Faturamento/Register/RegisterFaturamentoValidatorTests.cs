using BarberBossI.Application.UseCases.Faturamento;
using BarberBossI.Communication.Enums;
using BarberBossI.Exception;
using CommonTesteUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Faturamento.Register;

public class RegisterFaturamentoValidatorTests
{
	[Fact]
	public void Success()
	{
		var validator = new FaturamentoValidator();
		var request = RequestFaturamentoJsonBuilder.Build();

		var result = validator.Validate(request);

		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void ErrorTitleEmpty()
	{
		var validator = new FaturamentoValidator();
		var request = RequestFaturamentoJsonBuilder.Build();
		request.Titulo = string.Empty;

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.TituloObrigatorio));
	}

	[Fact]
	public void ErrorDateFuture()
	{
		var validator = new FaturamentoValidator();
		var request = RequestFaturamentoJsonBuilder.Build();
		request.Data = DateTime.Now.AddDays(1);

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.DataMaiorDataAtual));
	}

	[Fact]
	public void ErrorPaymentType()
	{
		var validator = new FaturamentoValidator();
		var request = RequestFaturamentoJsonBuilder.Build();
		request.TipoPagamento = (TipoPagamento)700;

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.TipoPagamentoInvalido));
	}

	[Fact]
	public void ErrorDescriptionEmpty()
	{
		var validator = new FaturamentoValidator();
		var request = RequestFaturamentoJsonBuilder.Build();
		request.Descricao = string.Empty;

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.DescricaoObrigatoria));
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	[InlineData(-2)]
	public void ErrorAmountInvalid(decimal valor)
	{
		var validator = new FaturamentoValidator();
		var request = RequestFaturamentoJsonBuilder.Build();
		request.Valor = valor;

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.ValorMaiorQueZero));
	}
}
