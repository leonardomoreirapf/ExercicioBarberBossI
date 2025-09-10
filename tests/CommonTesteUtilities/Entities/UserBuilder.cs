using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Enums;
using Bogus;
using CommonTesteUtilities.Cryptography;

namespace CommonTesteUtilities.Entities;

public class UserBuilder
{
	public static User Build(string role = Roles.TeamMember)
	{
		var passwordEncripter = new PasswordEncripterBuilder().Build();

		return new Faker<User>()
			.RuleFor(user => user.Id, _ => 1)
			.RuleFor(user => user.Name, faker => faker.Person.FirstName)
			.RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
			.RuleFor(user => user.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
			.RuleFor(user => user.UserIndentifier, _ => Guid.NewGuid())
			.RuleFor(user => user.Role, _ => role);
	}
}
