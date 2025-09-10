using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Enums;
using Bogus;

namespace CommonTesteUtilities.Entities;

public class FaturamentoBuilder
{
	public static List<Faturamento> Collection(User user, uint count = 2)
	{
		var list = new List<Faturamento>();

		if (count.Equals(0))
			count = 1;

		var faturamentoId = 1;

		for (int i = 0; i < count; i++)
		{
			var faturamento = Build(user);
			faturamento.Id = faturamentoId++;

			list.Add(faturamento);
		}

		return list;
	}
	public static Faturamento Build(User user)
	{
		return new Faker<Faturamento>()
			.RuleFor(u => u.Id, _ => 1)
			.RuleFor(u => u.Titulo, faker => faker.Commerce.ProductName())
			.RuleFor(u => u.Data, faker => faker.Date.Past())
			.RuleFor(u => u.TipoPagamento, faker => faker.PickRandom<TipoPagamento>())
			.RuleFor(u => u.Valor, faker => faker.Random.Decimal(min: 1, max: 1000))
			.RuleFor(u => u.Descricao, faker => faker.Commerce.ProductDescription())
			.RuleFor(u => u.UserId, _ => user.Id);
	}
}