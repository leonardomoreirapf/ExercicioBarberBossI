using BarberBossI.Communication.Requests;
using Bogus;
using System.Net.NetworkInformation;

namespace CommonTesteUtilities.Requests;

public class RequestLoginJsonBuilder
{
	public static RequestLoginJson Build()
	{
		return new Faker<RequestLoginJson>()
			.RuleFor(user => user.Email, faker => faker.Internet.Email())
			.RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
	}
}
