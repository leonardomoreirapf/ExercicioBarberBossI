﻿using BarberBossI.Domain.Repositories.User;
using Moq;

namespace CommonTesteUtilities.Repositories
{
	public class UserWriteOnlyRepositoryBuilder
	{
		public static IUserWriteOnlyRepository Build()
		{
			var mock = new Mock<IUserWriteOnlyRepository>();

			return mock.Object;
		}
	}
}
