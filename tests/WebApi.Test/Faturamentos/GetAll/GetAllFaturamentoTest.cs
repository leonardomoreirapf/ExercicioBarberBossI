using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Faturamentos.GetAll;

public class GetAllFaturamentoTest : BarberBossClassFixture
{
	private const string METHOD = "api/Faturamento";
	private readonly string _token;
	public GetAllFaturamentoTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_token = webApplicationFactory.UserTeamMember.GetToken();
	}

	[Fact]
	public async Task Sucess()
	{
		var result = await DoGet(requestUri: METHOD, token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.OK);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		response.RootElement.GetProperty("faturamentos").EnumerateArray().Should().NotBeNullOrEmpty();
	}
}
