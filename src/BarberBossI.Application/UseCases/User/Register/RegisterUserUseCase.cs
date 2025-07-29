using AutoMapper;
using BarberBossI.Communication.Requests;
using BarberBossI.Communication.Responses;
using BarberBossI.Domain.Repositories;
using BarberBossI.Domain.Repositories.User;
using BarberBossI.Domain.Security.Cryptography;
using BarberBossI.Domain.Security.Tokens;
using BarberBossI.Exception;
using BarberBossI.Exception.ExceptionsBase;

namespace BarberBossI.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
	private readonly IMapper _mapper;
	private readonly IPasswordEncripter _passwordEncripter;
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;
	private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAccessTokenGenerator _tokenGenerator;

	public RegisterUserUseCase(
		IMapper mapper, 
		IPasswordEncripter passwordEncripter, 
		IUserReadOnlyRepository userReadOnlyRepository,
		IUserWriteOnlyRepository userWriteOnlyRepository,
		IUnitOfWork unitOfWork,
		IAccessTokenGenerator tokenGenerator)
	{
		_mapper = mapper;
		_passwordEncripter = passwordEncripter;
		_userReadOnlyRepository = userReadOnlyRepository;
		_userWriteOnlyRepository = userWriteOnlyRepository;
		_unitOfWork = unitOfWork;
		_tokenGenerator = tokenGenerator;
	}

	public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
	{
		await Validate(request);

		var user = _mapper.Map<Domain.Entities.User>(request);
		user.Password = _passwordEncripter.Encrypt(request.Password);
		user.UserIndentifier = Guid.NewGuid();

		await _userWriteOnlyRepository.Add(user);
		await _unitOfWork.Commit();

		return new ResponseRegisterUserJson 
		{ 
			Name = user.Name,
			Token = _tokenGenerator.Generate(user)
		};
	}

	private async Task Validate(RequestRegisterUserJson request)
	{
		var result = new RegisterUserValidator().Validate(request);

		var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

		if (emailExist)
			result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessage.EmailExistente));
		

		if (!result.IsValid) 
		{
			var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

			throw new ErrorOnValidationException(errorMessages);
		}
	}
}
