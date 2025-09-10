using BarberBossI.Communication.Requests;
using BarberBossI.Exception;
using FluentValidation;

namespace BarberBossI.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
	public UpdateUserValidator()
	{
		RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessage.NomeVazio);
		RuleFor(user => user.Email)
			.NotEmpty()
			.WithMessage(ResourceErrorMessage.EmailVazio)
			.EmailAddress()
			.When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator)
			.WithMessage(ResourceErrorMessage.EmailInvalido);
	}
}
