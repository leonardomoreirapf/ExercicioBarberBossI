using BarberBossI.Communication.Requests;
using BarberBossI.Exception;
using CommonTesteUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Faturamentos.Update;

public class UpdateFaturamentoTest : BarberBossClassFixture
{
	private const string METHOD = "api/Faturamento";

	private readonly string _token;
	private readonly long _faturamentoId;

	public UpdateFaturamentoTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_token = webApplicationFactory.UserTeamMember.GetToken();
		_faturamentoId = webApplicationFactory.FaturamentoTeamMember.GetFaturamentoId();
	}


	[Fact]
	public async Task Sucess()
	{
		var request = RequestFaturamentoJsonBuilder.Build();
		var result = await DoPut(requestUri: $"{METHOD}/{_faturamentoId}", request: request , token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Error_Title_Empty()
	{
		var request = RequestFaturamentoJsonBuilder.Build();
		request.Titulo = string.Empty;

		var result = await DoPut(requestUri: $"{METHOD}/{_faturamentoId}", request: request, token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessage.TituloObrigatorio));
	}

	[Fact]
	public async Task Error_Faturamento_Not_Found()
	{
		var request = RequestFaturamentoJsonBuilder.Build();

		var result = await DoPut(requestUri: $"{METHOD}/10000", request: request, token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.NotFound);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessage.FaturamentoNaoEncontrado));
	}
}
