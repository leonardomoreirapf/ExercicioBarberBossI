using BarberBossI.Communication.Requests;
using Bogus;
using System.Net.NetworkInformation;

namespace CommonTesteUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
	public static RequestRegisterUserJson Build()
	{
		return new Faker<RequestRegisterUserJson>()
			.RuleFor(user => user.Name, faker => faker.Person.FirstName)
			.RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(firstName: user.Name))
			.RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
	}
}
