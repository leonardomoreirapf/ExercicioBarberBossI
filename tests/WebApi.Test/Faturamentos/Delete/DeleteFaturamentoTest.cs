using BarberBossI.Exception;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Faturamentos.Delete;

public class DeleteFaturamentoTest : BarberBossClassFixture
{
	private const string METHOD = "api/Faturamento";

	private readonly string _token;
	private readonly long _faturamentoId;
	public DeleteFaturamentoTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_token = webApplicationFactory.UserTeamMember.GetToken();
		_faturamentoId = webApplicationFactory.FaturamentoTeamMember.GetFaturamentoId();
	}

	[Fact]
	public async Task Sucess()
	{
		var result = await DoDelete(requestUri: $"{METHOD}/{_faturamentoId}", token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.NoContent);

		result = await DoGet(requestUri: $"{METHOD}/{_faturamentoId}", token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Error_Faturamento_Not_Found()
	{
		var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.NotFound);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessage.FaturamentoNaoEncontrado));
	}
}
