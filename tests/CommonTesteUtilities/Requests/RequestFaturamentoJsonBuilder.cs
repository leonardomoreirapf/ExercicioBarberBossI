using BarberBossI.Communication.Enums;
using BarberBossI.Communication.Requests;
using Bogus;

namespace CommonTesteUtilities.Requests;

public class RequestFaturamentoJsonBuilder
{
	public static RequestFaturamentoJson Build()
	{
		return new Faker<RequestFaturamentoJson>()
				.RuleFor(faturamento => faturamento.Titulo, faker => faker.Commerce.ProductName())
				.RuleFor(faturamento => faturamento.Data, faker => faker.Date.Past())
				.RuleFor(faturamento => faturamento.TipoPagamento, faker => faker.PickRandom<TipoPagamento>())
				.RuleFor(faturamento => faturamento.Valor, faker => faker.Random.Decimal(min: 1, max: 10000))
				.RuleFor(faturamento => faturamento.Descricao, faker => faker.Commerce.ProductDescription());
	}
}
