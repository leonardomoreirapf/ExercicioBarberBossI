using BarberBossI.Communication.Responses;

namespace BarberBossI.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
	Task<ResponseUserProfileJson> Execute();
}
