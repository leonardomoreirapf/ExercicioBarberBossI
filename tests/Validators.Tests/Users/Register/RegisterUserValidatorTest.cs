using BarberBossI.Application.UseCases.User.Register;
using BarberBossI.Exception;
using CommonTesteUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTest
{
	[Fact]
	public void Sucess()
	{
		var validator = new RegisterUserValidator();
		var request = RequestRegisterUserJsonBuilder.Build();

		var result = validator.Validate(request);

		result.IsValid.Should().BeTrue();
	}

	[Theory]
	[InlineData("")]
	[InlineData("      ")]
	[InlineData(null)]
	public void Error_Name_Empty(string name)
	{
		var validator = new RegisterUserValidator();
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Name = name;

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.NomeVazio));
	}

	[Theory]
	[InlineData("")]
	[InlineData("      ")]
	[InlineData(null)]
	public void Error_Email_Empty(string email)
	{
		var validator = new RegisterUserValidator();
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Email = email;

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.EmailVazio));
	}

	[Fact]
	public void Error_Email_Invalid()
	{
		var validator = new RegisterUserValidator();
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Email = "leonardo.com";

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.EmailInvalido));
	}

	[Fact]
	public void Error_Password_Empty()
	{
		var validator = new RegisterUserValidator();
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Password = string.Empty;

		var result = validator.Validate(request);

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessage.SenhaInvalida));
	}
}
