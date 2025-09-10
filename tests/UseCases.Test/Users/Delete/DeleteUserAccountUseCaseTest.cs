using BarberBossI.Application.UseCases.User.Delete;
using BarberBossI.Domain.Entities;
using CommonTesteUtilities;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Users.Delete;

public class DeleteUserAccountUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var user = UserBuilder.Build();
		var useCase = CreateUseCase(user);

		var act = async () => await useCase.Execute();

		await act.Should().NotThrowAsync();
	}

	private DeleteUserAccountUseCase CreateUseCase(User user)
	{
		var repository = UserWriteOnlyRepositoryBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);
		var unitOfWord = UnitOfWorkBuilder.Build();

		return new DeleteUserAccountUseCase(loggedUser, repository, unitOfWord);
	}
}
