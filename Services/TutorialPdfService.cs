using HuongDanLamDep.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HuongDanLamDep.Services
{
	public interface ITutorialPdfService
	{
		byte[] GenerateTutorialPdf(Tutorial tutorial);
	}

	public class TutorialPdfService : ITutorialPdfService
	{
		public byte[] GenerateTutorialPdf(Tutorial tutorial)
		{
			var doc = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Size(PageSizes.A4);
					page.Margin(25);
					page.DefaultTextStyle(x => x.FontSize(12));

					page.Header().Column(col =>
					{
						col.Item().Text("WEB HƯỚNG DẪN LÀM ĐẸP").SemiBold().FontSize(16);
						col.Item().Text($"Tutorial: {tutorial.Title}").SemiBold().FontSize(14);
						col.Item().Text($"Category: {tutorial.Category?.Name ?? "N/A"}");
						col.Item().LineHorizontal(1);
					});

					page.Content().PaddingVertical(10).Column(col =>
					{
						col.Spacing(8);
						col.Item().Text("Nội dung").SemiBold().FontSize(13);
						col.Item().Text(tutorial.Content ?? "");
						col.Item().PaddingTop(10).LineHorizontal(1);
						col.Item().Text($"Xuất lúc: {DateTime.Now:dd/MM/yyyy HH:mm}");
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
	}
}