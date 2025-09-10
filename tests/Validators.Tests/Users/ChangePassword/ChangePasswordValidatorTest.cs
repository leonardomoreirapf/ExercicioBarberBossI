using BarberBossI.Application.UseCases.User.ChangePassword;
using BarberBossI.Exception;
using CommonTesteUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.ChangePassword;

public class ChangePasswordValidatorTest
{
	[Fact]
	public void Sucess()
	{
		var validator = new ChangePasswordValidator();

		var request = RequestChangePasswordJsonBuilder.Build();

		var result = validator.Validate(request);

		result.IsValid.Should().BeTrue();
	}

	[Theory]
	[InlineData("")]
	[InlineData("      ")]
	[InlineData(null)]
	public void Error_NewPassword_Empty(string newPassword)
	{
		var validator = new ChangePasswordValidator();

		var request = RequestChangePasswordJsonBuilder.Build();
		request.NewPassword = newPassword;

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.SenhaInvalida));
	}
}
