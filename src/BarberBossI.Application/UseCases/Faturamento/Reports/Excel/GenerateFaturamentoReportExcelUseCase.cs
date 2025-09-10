using BarberBossI.Domain.Enums;
using BarberBossI.Domain.Extensions;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Domain.Services.LoggedUser;
using ClosedXML.Excel;

namespace BarberBossI.Application.UseCases.Faturamento.Reports.Excel;

public class GenerateFaturamentoReportExcelUseCase : IGenerateFaturamentoReportExcelUseCase
{
	private const string CURRENCY_SYMBOL = "R$";
	private IFaturamentoReadOnlyRepository _repository;
	private readonly ILoggedUser _loggedUser;

	public GenerateFaturamentoReportExcelUseCase(
		IFaturamentoReadOnlyRepository repository,
		ILoggedUser loggedUser)
	{
		_repository = repository;
		_loggedUser = loggedUser;
	}
	public async Task<byte[]> Execute(DateOnly month)
	{
		var loggedUser = await _loggedUser.Get();

		var faturamentos = await _repository.FilterByMonth(loggedUser, month);

		if (faturamentos.Count.Equals(0))
			return [];

		using var workbook = new XLWorkbook();

		workbook.Author = loggedUser.Name;
		workbook.Style.Font.FontSize = 12;
		workbook.Style.Font.FontName = "Times New Roman";

		var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

		InsertHeaders(worksheet);
		InsertRows(worksheet, faturamentos);

		worksheet.Columns().AdjustToContents();

		var file = new MemoryStream();

		workbook.SaveAs(file);

		return file.ToArray();
	}

	private void InsertRows(IXLWorksheet worksheet, List<Domain.Entities.Faturamento> faturamentos, int row = 2)
	{
		foreach(var faturamento in faturamentos)
		{
			worksheet.Cell($"A{row}").Value = faturamento.Titulo;
			worksheet.Cell($"A{row}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

			worksheet.Cell($"B{row}").Value = faturamento.Data;
			worksheet.Cell($"B{row}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

			worksheet.Cell($"C{row}").Value = faturamento.TipoPagamento.TipoPagamentoToString();
			worksheet.Cell($"C{row}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

			worksheet.Cell($"D{row}").Value = faturamento.Valor;
			worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"{CURRENCY_SYMBOL} #,##0.00";
			worksheet.Cell($"D{row}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

			worksheet.Cell($"E{row}").Value = faturamento.Descricao;
			worksheet.Cell($"E{row}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

			row++;
		}

	}

	private void InsertHeaders(IXLWorksheet worksheet)
	{
		worksheet.Cell("A1").Value = "Título";
		worksheet.Cell("B1").Value = "Data";
		worksheet.Cell("C1").Value = "Tipo de pagamento";
		worksheet.Cell("D1").Value = "Valor";
		worksheet.Cell("E1").Value = "Descrição";

		worksheet.Cells("A1:E1").Style.Font.Bold = true;
		worksheet.Cells("A1:E1").Style.Font.FontColor = XLColor.White;

		worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#205858");

		worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
		worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
		worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
		worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
		worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
	}
}
