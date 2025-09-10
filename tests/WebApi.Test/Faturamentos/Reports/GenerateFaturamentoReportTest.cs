using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Faturamentos.Reports;

public class GenerateFaturamentoReportTest : BarberBossClassFixture
{
	private const string METHOD = "api/Report";

	private readonly string _adminToken;
	private readonly string _teamMemberToken;
	private readonly DateTime _faturamentoData;

	public GenerateFaturamentoReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_adminToken = webApplicationFactory.UserAdmin.GetToken();
		_teamMemberToken = webApplicationFactory.UserTeamMember.GetToken();
		_faturamentoData = webApplicationFactory.FaturamentoAdmin.GetDate();
	}

	[Fact]
	public async Task Sucess_Pdf()
	{
		var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_faturamentoData:yyyy-MM}", token: _adminToken);

		result.StatusCode.Should().Be(HttpStatusCode.OK);

		result.Content.Headers.ContentType.Should().NotBeNull();
		result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
	}

	[Fact]
	public async Task Sucess_Excel()
	{
		var result = await DoGet(requestUri: $"{METHOD}/excel?month={_faturamentoData:yyyy-MM}", token: _adminToken);

		result.StatusCode.Should().Be(HttpStatusCode.OK);

		result.Content.Headers.ContentType.Should().NotBeNull();
		result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
	}

	[Fact]
	public async Task Error_Forbidden_User_not_Allowed_Excel()
	{
		var result = await DoGet(requestUri: $"{METHOD}/excel?month={_faturamentoData:Y}", token: _teamMemberToken);

		result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
	}

	[Fact]
	public async Task Error_Forbidden_User_not_Allowed_Pdf()
	{
		var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_faturamentoData:Y}", token: _teamMemberToken);

		result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
	}
}
