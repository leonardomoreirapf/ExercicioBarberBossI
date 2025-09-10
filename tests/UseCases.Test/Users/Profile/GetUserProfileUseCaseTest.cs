using BarberBossI.Application.UseCases.User.Profile;
using BarberBossI.Domain.Entities;
using CommonTesteUtilities;
using CommonTesteUtilities.Entities;
using CommonTesteUtilities.Mapper;
using FluentAssertions;

namespace UseCases.Test.Users.Profile;

public class GetUserProfileUseCaseTest
{
	[Fact]
	public async Task Sucess()
	{
		var user = UserBuilder.Build();
		var useCase = CreateUseCase(user);

		var result = await useCase.Execute();

		result.Should().NotBeNull();
		result.Name.Should().Be(user.Name);
		result.Email.Should().Be(user.Email);
	}
	private GetUserProfileUseCase CreateUseCase(User user)
	{
		var mapper = MapperBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new GetUserProfileUseCase(mapper, loggedUser);
	}
}
