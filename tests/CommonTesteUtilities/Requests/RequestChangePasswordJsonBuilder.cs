using BarberBossI.Communication.Requests;
using Bogus;

namespace CommonTesteUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
	public static RequestChangePasswordJson Build()
	{
		return new Faker<RequestChangePasswordJson>()
			.RuleFor(user => user.Password, faker => faker.Internet.Password())
			.RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1", length:10));
	}
}
