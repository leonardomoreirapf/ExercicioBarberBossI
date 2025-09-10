﻿using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Security.Tokens;
using Moq;

namespace CommonTesteUtilities.Token;

public class JwtTokenGeneratorBuilder
{
	public static IAccessTokenGenerator Build()
	{
		var mock = new Mock<IAccessTokenGenerator>();

		mock.Setup(config => config.Generate(It.IsAny<User>())).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30");

		return mock.Object;
	}
}
