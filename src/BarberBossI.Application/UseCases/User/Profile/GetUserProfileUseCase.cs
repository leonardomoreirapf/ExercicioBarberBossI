using AutoMapper;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Services.LoggedUser;

namespace BarberBossI.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
	private readonly IMapper _mapper;
	private readonly ILoggedUser _loggedUser;

	public GetUserProfileUseCase(IMapper mapper, ILoggedUser loggedUser)
	{
		_mapper = mapper;
		_loggedUser = loggedUser;
	}

	public async Task<ResponseUserProfileJson> Execute()
	{
		var user = await _loggedUser.Get();

		return _mapper.Map<ResponseUserProfileJson>(user);
	}
}
