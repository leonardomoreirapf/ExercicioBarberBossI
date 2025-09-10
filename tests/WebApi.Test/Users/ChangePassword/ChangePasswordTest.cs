using BarberBossI.Communication.Requests;
using BarberBossI.Exception;
using CommonTesteUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.ChangePassword;

public class ChangePasswordTest : BarberBossClassFixture
{
	private const string METHOD = "api/User/change-password";

	private readonly string _token;
	private readonly string _password;
	private readonly string _email;

	public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_token = webApplicationFactory.UserTeamMember.GetToken();
		_email = webApplicationFactory.UserTeamMember.GetEmail();
		_password = webApplicationFactory.UserTeamMember.GetPassword();
	}

	[Fact]
	public async Task Sucess()
	{
		var request = RequestChangePasswordJsonBuilder.Build();
		request.Password = _password;

		var response = await DoPut(requestUri: METHOD, request: request, token: _token);

		response.StatusCode.Should().Be(HttpStatusCode.NoContent);

		var loginRequest = new RequestLoginJson
		{
			Email = _email,
			Password = _password
		};

		response = await DoPost("api/Login", loginRequest);
		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		loginRequest.Password = request.NewPassword;

		response = await DoPost("api/Login", loginRequest);
		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Error_CurrentPassword_Different()
	{
		var request = RequestChangePasswordJsonBuilder.Build();

		var response = await DoPut(requestUri: METHOD, request: request, token: _token);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		await using var responseBody = await response.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responseBody);

		var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessage.SenhaDiferenteDaAtual));
	}
}
