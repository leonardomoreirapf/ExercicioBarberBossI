using BarberBossI.Exception;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace BarberBossI.Application.UseCases.User;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
	private const string ErrorMessageKey = "ErrorMessage";
	public override string Name => "PasswordValidator";

	protected override string GetDefaultMessageTemplate(string errorCode)
	{
		return $"{{{ErrorMessageKey}}}";
	}

	public override bool IsValid(ValidationContext<T> context, string password)
	{
		if (string.IsNullOrWhiteSpace(password))
		{
			context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessage.SenhaInvalida);
			return false;
		}

		if (password.Length < 8)
		{
			context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessage.SenhaInvalida);
			return false;
		}

		if (!Regex.IsMatch(password, @"[A-Z]+"))
		{
			context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessage.SenhaInvalida);
			return false;
		}

		if (!Regex.IsMatch(password, @"[a-z]+"))
		{
			context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessage.SenhaInvalida);
			return false;
		}

		if (!Regex.IsMatch(password, @"[0-9]+"))
		{
			context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessage.SenhaInvalida);
			return false;
		}

		if (!Regex.IsMatch(password, @"[\!\?\*\.]+"))
		{
			context.MessageFormatter.AppendArgument(ErrorMessageKey, ResourceErrorMessage.SenhaInvalida);
			return false;
		}

		return true;
	}
}
