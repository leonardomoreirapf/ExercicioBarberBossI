using BarberBossI.Communication.Enums;
using BarberBossI.Exception;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Faturamentos.GetById;

public class GetFaturamentoByIdTest : BarberBossClassFixture
{
	private const string METHOD = "api/Faturamento";

	private readonly string _token;
	private readonly long _faturamentoId;

	public GetFaturamentoByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_token = webApplicationFactory.UserTeamMember.GetToken();
		_faturamentoId = webApplicationFactory.FaturamentoTeamMember.GetFaturamentoId();
	}

	[Fact]
	public async Task Sucess()
	{
		var result = await DoGet(requestUri: $"{METHOD}/{_faturamentoId}", token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.OK);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		response.RootElement.GetProperty("id").GetInt64().Should().Be(_faturamentoId);
		response.RootElement.GetProperty("titulo").GetString().Should().NotBeNullOrWhiteSpace();
		response.RootElement.GetProperty("descricao").GetString().Should().NotBeNullOrWhiteSpace();
		response.RootElement.GetProperty("data").GetDateTime().Should().NotBeAfter(DateTime.Today);
		response.RootElement.GetProperty("valor").GetDecimal().Should().BeGreaterThan(0);

		var tipoPagamento = response.RootElement.GetProperty("tipoPagamento").GetInt32();
		Enum.IsDefined(typeof(TipoPagamento), tipoPagamento).Should().BeTrue();
	}

	[Fact]
	public async Task Error_Faturamento_Not_Found()
	{
		var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.NotFound);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessage.FaturamentoNaoEncontrado));
	}
}
