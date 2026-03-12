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
			var document = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Size(PageSizes.A4);
					page.Margin(25);
					page.PageColor(Colors.White);
					page.DefaultTextStyle(x => x.FontSize(12));

					// Header
					page.Header().Column(col =>
					{
						col.Spacing(6);

						col.Item().Text("LUMBEAUTY")
							.FontSize(18)
							.Bold()
							.FontColor(Colors.Pink.Darken2);

						col.Item().Text(tutorial.Title ?? "")
							.FontSize(16)
							.Bold();

						col.Item().Text($"Danh mục: {tutorial.Category?.Name ?? "Chưa có"}")
							.FontSize(11);

						col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
					});

					// Content
					page.Content().PaddingVertical(10).Column(col =>
					{
						col.Spacing(10);

						col.Item().Text("Nội dung")
							.FontSize(13)
							.SemiBold();

						// Hiển thị đầy đủ, giữ xuống dòng
						col.Item().Text(tutorial.Content ?? "")
							.FontSize(11)
							.LineHeight(1.5f);
					});

					// Footer
					page.Footer().AlignCenter().Text(x =>
					{
						x.Span("Trang ");
						x.CurrentPageNumber();
						x.Span(" / ");
						x.TotalPages();
					});
				});
			});

			return document.GeneratePdf();
		}
	}
}
