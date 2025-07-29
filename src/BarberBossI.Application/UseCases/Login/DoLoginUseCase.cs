using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Repositories.User;
using BarberBossI.Domain.Security.Cryptography;
using BarberBossI.Domain.Security.Tokens;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.Login;

public class DoLoginUseCase : IDoLoginUseCase
{
	private readonly IUserReadOnlyRepository _repository;
	private readonly IPasswordEncripter _passwordEncripter;
	private readonly IAccessTokenGenerator _accessTokenGenerator;

	public DoLoginUseCase(IUserReadOnlyRepository repository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
	{
		_repository = repository;
		_passwordEncripter = passwordEncripter;
		_accessTokenGenerator = accessTokenGenerator;
	}
	public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
	{
		var user = await _repository.GetUserByEmail(request.Email);

		if (user is null)
			throw new InvalidLoginException();

		var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

		if(!passwordMatch)
			throw new InvalidLoginException();

		return new ResponseRegisterUserJson
		{
			Name = user.Name,
			Token = _accessTokenGenerator.Generate(user)
		};
	}
}
