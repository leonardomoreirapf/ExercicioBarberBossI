﻿using BarberBossI.Communication.Requests;
using Bogus;

namespace CommonTesteUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
	public static RequestUpdateUserJson Build()
	{
		return new Faker<RequestUpdateUserJson>()
			.RuleFor(user => user.Name, faker => faker.Person.FirstName)
			.RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
	}
}
