using BarberBossI.Communication.Requests;
using BarberBossI.Exception;
using CommonTesteUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
{
	private const string METHOD = "api/Login";
	private readonly HttpClient _httpClient;
	private readonly string _email;
	private readonly string _name;
	private readonly string _password;

	public DoLoginTest(CustomWebApplicationFactory webApplicationFactory)
	{
		_httpClient = webApplicationFactory.CreateClient();
		_email = webApplicationFactory.UserTeamMember.GetEmail();
		_name = webApplicationFactory.UserTeamMember.GetName();
		_password = webApplicationFactory.UserTeamMember.GetPassword();
	}

	[Fact]
	public async Task Sucess()
	{
		var request = new RequestLoginJson
		{
			Email = _email,
			Password = _password
		};

		var result = await _httpClient.PostAsJsonAsync(METHOD, request);

		result.StatusCode.Should().Be(HttpStatusCode.OK);

		var responseBody = await result.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responseBody);

		responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
		responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Error_Login_Invalid()
	{
		var request = RequestLoginJsonBuilder.Build();

		var result = await _httpClient.PostAsJsonAsync(METHOD, request);

		result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

		var responseBody = await result.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responseBody);

		var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessage.LoginOuSenhaInvalido));
	}
}