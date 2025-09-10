﻿using BarberBossI.Domain.Entities;
using BarberBossI.Domain.Repositories.User;
using Moq;

namespace CommonTesteUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
	private readonly Mock<IUserReadOnlyRepository> _repository;

	public UserReadOnlyRepositoryBuilder()
	{
		_repository = new Mock<IUserReadOnlyRepository>();
	}

	public void ExistActiveUserWithEmail(string email)
	{
		_repository.Setup(userReadyOnly => userReadyOnly.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
	}

	public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
	{
		_repository.Setup(userReadyOnly => userReadyOnly.GetUserByEmail(user.Email)).ReturnsAsync(user);
		
		return this;
	}

	public  IUserReadOnlyRepository Build() => _repository.Object;
}
