using BarberBossI.Application.UseCases.Faturamento.Reports.Excel;
using BarberBossI.Application.UseCases.Faturamento.Reports.Pdf;
using BarberBossI.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BarberBossI.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = Roles.Admin)]
	public class ReportController : ControllerBase
	{
		[HttpGet("excel")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> GetExcel(
			[FromServices] IGenerateFaturamentoReportExcelUseCase useCase,
			[FromQuery] DateOnly month)
		{
			var file = await useCase.Execute(month);

			if (file.Length > 0)
				return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

			return NoContent();
		}

		[HttpGet("pdf")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> GetPdf(
			[FromServices] IGenerateFaturamentoReportPdfUseCase useCase,
			[FromQuery] DateOnly month)
		{
			var file = await useCase.Execute(month);

			if (file.Length > 0)
				return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

			return NoContent();
		}
	}
}
