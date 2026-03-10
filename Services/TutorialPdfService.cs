using HuongDanLamDep.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HuongDanLamDep.Services
{
	public interface ITutorialReportPdfService
	{
		byte[] GenerateCategoryReport(string categoryName, IList<Tutorial> tutorials);
	}

	public class TutorialReportPdfService : ITutorialReportPdfService
	{
		public byte[] GenerateCategoryReport(string categoryName, IList<Tutorial> tutorials)
		{
			var doc = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Size(PageSizes.A4);
					page.Margin(25);
					page.DefaultTextStyle(x => x.FontSize(11));

					page.Header().Column(col =>
					{
						col.Item().Text("WEB HƯỚNG DẪN LÀM ĐẸP").SemiBold().FontSize(16);
						col.Item().Text("BÁO CÁO TUTORIAL THEO CATEGORY").SemiBold().FontSize(13);
						col.Item().Text($"Category: {categoryName}");
						col.Item().Text($"Tổng số: {tutorials.Count}");
						col.Item().LineHorizontal(1);
					});

					page.Content().PaddingVertical(10).Table(table =>
					{
						table.ColumnsDefinition(cols =>
						{
							cols.ConstantColumn(30);   // STT
							cols.RelativeColumn(3);    // Title
							cols.RelativeColumn(2);    // Preview
						});

						table.Header(header =>
						{
							header.Cell().Element(HeaderCell).Text("STT");
							header.Cell().Element(HeaderCell).Text("Tiêu đề");
							header.Cell().Element(HeaderCell).Text("Mô tả ngắn");
						});

						for (int i = 0; i < tutorials.Count; i++)
						{
							var t = tutorials[i];
							table.Cell().Element(BodyCell).Text((i + 1).ToString());
							table.Cell().Element(BodyCell).Text(t.Title ?? "");
							table.Cell().Element(BodyCell).Text(Shorten(t.Content, 120));
						}

						static IContainer HeaderCell(IContainer c) =>
							c.Background(Colors.Grey.Lighten3).Padding(6)
							 .Border(1).BorderColor(Colors.Grey.Medium)
							 .DefaultTextStyle(x => x.SemiBold());

						static IContainer BodyCell(IContainer c) =>
							c.Padding(6).Border(1).BorderColor(Colors.Grey.Lighten2);
					});

					page.Footer().AlignCenter().Text(x =>
					{
						x.Span("Trang ");
						x.CurrentPageNumber();
						x.Span(" / ");
						x.TotalPages();
					});
				});
			});

			return doc.GeneratePdf();
		}

		private static string Shorten(string? text, int max)
		{
			if (string.IsNullOrWhiteSpace(text)) return "";
			text = text.Replace("\r", " ").Replace("\n", " ").Trim();
			return text.Length <= max ? text : text.Substring(0, max) + "...";
		}
	}
}