using BarberBossI.Communication.Requests;
using BarberBossI.Exception;
using FluentValidation;

namespace BarberBossI.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
	public RegisterUserValidator()
	{
		RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessage.NomeVazio);
		RuleFor(user => user.Email)
			.NotEmpty()
			.WithMessage(ResourceErrorMessage.EmailVazio)
			.EmailAddress()
			.When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator)
			.WithMessage(ResourceErrorMessage.EmailInvalido);
		RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
	}
}
