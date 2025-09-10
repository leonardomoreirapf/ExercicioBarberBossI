using BarberBossI.Exception;
using CommonTesteUtilities.Requests;
using FluentAssertions;
using System.Text.Json;

namespace WebApi.Test.Faturamentos.Register;

public class RegisterFaturamentoTest : BarberBossClassFixture
{
	private const string METHOD = "api/Faturamento";
	private readonly string _token;

	public RegisterFaturamentoTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_token = webApplicationFactory.UserTeamMember.GetToken();
	}

	[Fact]
	public async Task Sucess()
	{
		var request = RequestFaturamentoJsonBuilder.Build();

		var result = await DoPost(METHOD, request, _token);

		result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		response.RootElement.GetProperty("titulo").GetString().Should().Be(request.Titulo);
	}

	[Fact]
	public async Task Error_Title_Empty()
	{
		var request = RequestFaturamentoJsonBuilder.Build();
		request.Titulo = string.Empty;

		var result = await DoPost(METHOD, request, _token); ;

		result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessage.TituloObrigatorio));
	}
}
