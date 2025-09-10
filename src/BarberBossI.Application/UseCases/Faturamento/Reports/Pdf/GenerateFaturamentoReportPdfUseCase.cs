
using BarberBossI.Application.UseCases.Faturamento.Reports.Pdf.Colors;
using BarberBossI.Application.UseCases.Faturamento.Reports.Pdf.Fonts;
using BarberBossI.Domain.Extensions;
using BarberBossI.Domain.Repositories.Faturamentos;
using BarberBossI.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBossI.Application.UseCases.Faturamento.Reports.Pdf;

public class GenerateFaturamentoReportPdfUseCase : IGenerateFaturamentoReportPdfUseCase
{
	private const int HEIGHT_ROW_DEFAULT_TABLE = 25;
	private readonly IFaturamentoReadOnlyRepository _repository;
	private readonly ILoggedUser _loggedUser;

	public GenerateFaturamentoReportPdfUseCase(
		IFaturamentoReadOnlyRepository repository,
		ILoggedUser loggedUser)
	{
		_repository = repository;
		_loggedUser = loggedUser;

		GlobalFontSettings.FontResolver = new FaturamentoReportFontResolver();
	}
	public async Task<byte[]> Execute(DateOnly month)
	{
		var loggedUser = await _loggedUser.Get();

		var faturamentos = await _repository.FilterByMonth(loggedUser, month);

		if (faturamentos.Count.Equals(0))
			return [];

		var document = CreateDocument(loggedUser.Name, month);
		var page = CreatePage(document);

		CreateHeaderWithProfilePhoto(page);

		var total = faturamentos.Sum(Faturamento => Faturamento.Valor);

		CreateTotalFaturamentoSection(total, page);

		foreach(var faturamento in faturamentos)
		{
			var table = CreateFaturamentoTable(page);

			var row = table.AddRow();
			row.Height = HEIGHT_ROW_DEFAULT_TABLE;

			AddFaturamentoTitle(row.Cells[0], faturamento.Titulo);
			AddHeaderForAmount(row.Cells[3]);

			row = table.AddRow();
			row.Height = HEIGHT_ROW_DEFAULT_TABLE;

			row.Cells[0].AddParagraph(faturamento.Data.ToString("D"));
			SetStyleBaseForFaturamentoInformation(row.Cells[0]);
			row.Cells[0].Format.LeftIndent = 9;

			row.Cells[1].AddParagraph(faturamento.Data.ToString("t"));
			SetStyleBaseForFaturamentoInformation(row.Cells[1]);

			row.Cells[2].AddParagraph(faturamento.TipoPagamento.TipoPagamentoToString());
			SetStyleBaseForFaturamentoInformation(row.Cells[2]);

			AddAmountForFaturamento(row.Cells[3], faturamento.Valor.ToString("C"));

			if (!string.IsNullOrWhiteSpace(faturamento.Descricao))
			{
				var descriptionRow = table.AddRow();
				descriptionRow.Height = HEIGHT_ROW_DEFAULT_TABLE;
				descriptionRow.Cells[0].AddParagraph(faturamento.Descricao);
				descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 9, Color = ColorsHelper.GRAY };
				descriptionRow.Cells[0].Shading.Color = ColorsHelper.WHITE_LIGHT;
				descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
				descriptionRow.Cells[0].MergeRight = 2;
				descriptionRow.Cells[0].Format.LeftIndent = 9;

				row.Cells[3].MergeDown = 1;
			}

			AddWhiteSpace(table);

		}

		return RenderDocument(document);
	}

	private void AddWhiteSpace(Table table)
	{
		var row = table.AddRow();
		row.Height = 16;
		row.Borders.Visible = false;
	}

	private void AddAmountForFaturamento(Cell cell, string valor)
	{
		cell.AddParagraph(valor);
		cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
		cell.Shading.Color = ColorsHelper.WHITE;
		cell.VerticalAlignment = VerticalAlignment.Center;
	}

	private static void CreateTotalFaturamentoSection(decimal total, Section page)
	{
		var paragraph = page.AddParagraph();
		paragraph.Format.SpaceBefore = "38";
		paragraph.Format.SpaceAfter = "38";


		paragraph.AddFormattedText("Faturamento da semana", new Font { Name = FontHelper.ROBOTO_MEDIUM, Size = 15 });
		paragraph.AddLineBreak();


		paragraph.AddFormattedText($"{total:C}", new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 50 });
	}

	private static void CreateHeaderWithProfilePhoto(Section page)
	{
		var table = page.AddTable();
		table.AddColumn();
		table.AddColumn("300");

		var row = table.AddRow();

		var assembly = Assembly.GetExecutingAssembly();
		var directory = Path.GetDirectoryName(assembly.Location);
		row.Cells[0].AddImage(Path.Combine(directory!, "Images", "barberboss.png"));
		row.Cells[1].AddParagraph("BARBEARIA DO JOÃO");
		row.Cells[1].Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 25 };
		row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
		row.Cells[1].Format.LeftIndent = 18;
	}

	private Document CreateDocument(string author, DateOnly month)
	{
		var document = new Document();

		document.Info.Title = $"Despesas de {month:Y}";
		document.Info.Author = author;

		var style = document.Styles["Normal"];
		style!.Font.Name = FontHelper.ROBOTO_REGULAR;

		return document;
	}

	private Section CreatePage(Document document)
	{
		var section = document.AddSection();
		section.PageSetup = document.DefaultPageSetup.Clone();

		section.PageSetup.PageFormat = PageFormat.A4;
		section.PageSetup.LeftMargin = 40;
		section.PageSetup.RightMargin = 40;
		section.PageSetup.TopMargin = 80;
		section.PageSetup.BottomMargin = 80;

		return section;
	}

	private byte[] RenderDocument(Document document) 
	{
		var renderer = new PdfDocumentRenderer
		{
			Document = document
		};

		renderer.RenderDocument();

		using var file = new MemoryStream();
		renderer.PdfDocument.Save(file);

		return file.ToArray();
	}

	private Table CreateFaturamentoTable(Section page)
	{
		var table = page.AddTable();
		table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
		table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
		table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
		table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;


		return table;
	}

	private void AddFaturamentoTitle(Cell cell, string title)
	{
		cell.AddParagraph(title);
		cell.Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
		cell.Shading.Color = ColorsHelper.GREEN_DARK;
		cell.VerticalAlignment = VerticalAlignment.Center;
		cell.MergeRight = 2;
		cell.Format.LeftIndent = 9;
	}

	private void AddHeaderForAmount(Cell cell)
	{
		cell.AddParagraph("Valor");
		cell.Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
		cell.Shading.Color = ColorsHelper.GREEN_LIGHT;
		cell.VerticalAlignment = VerticalAlignment.Center;
	}

	private void SetStyleBaseForFaturamentoInformation(Cell cell)
	{
		cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
		cell.Shading.Color = ColorsHelper.GRAY_LIGHT;
		cell.VerticalAlignment = VerticalAlignment.Center;
	}
}
