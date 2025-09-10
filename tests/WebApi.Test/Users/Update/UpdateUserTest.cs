using BarberBossI.Exception;
using CommonTesteUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace WebApi.Test.Users.Update;

public class UpdateUserTest : BarberBossClassFixture
{
	private const string METHOD = "api/User";

	private readonly string _token;

	public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_token = webApplicationFactory.UserTeamMember.GetToken();
	}

	[Fact]
	public async Task Sucess()
	{
		var request = RequestUpdateUserJsonBuilder.Build();

		var response = await DoPut(requestUri: METHOD, request: request, token: _token);

		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Error_Empty_Name()
	{
		var request = RequestUpdateUserJsonBuilder.Build();
		request.Name = string.Empty;

		var response = await DoPut(requestUri: METHOD, request: request, token: _token);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		await using var responseBody = await response.Content.ReadAsStreamAsync();

		var responseData = await JsonDocument.ParseAsync(responseBody);

		var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessage.NomeVazio));
	}
}
