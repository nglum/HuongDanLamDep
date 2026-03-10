using ClosedXML.Excel;
using HuongDanLamDep.Models;

namespace HuongDanLamDep.Services
{
	public interface ITutorialReportExcelService
	{
		byte[] GenerateCategoryReport(string categoryName, IList<Tutorial> tutorials);
	}

	public class TutorialReportExcelService : ITutorialReportExcelService
	{
		public byte[] GenerateCategoryReport(string categoryName, IList<Tutorial> tutorials)
		{
			using var wb = new XLWorkbook();
			var ws = wb.Worksheets.Add("Tutorials");

			ws.Cell(1, 1).Value = "BÁO CÁO TUTORIAL THEO CATEGORY";
			ws.Cell(2, 1).Value = "Category:";
			ws.Cell(2, 2).Value = categoryName;
			ws.Cell(3, 1).Value = "Tổng số:";
			ws.Cell(3, 2).Value = tutorials.Count;

			// Header bảng (row 5)
			ws.Cell(5, 1).Value = "STT";
			ws.Cell(5, 2).Value = "Tiêu đề";
			ws.Cell(5, 3).Value = "Mô tả ngắn";

			ws.Range(5, 1, 5, 3).Style.Font.Bold = true;
			ws.Range(5, 1, 5, 3).Style.Fill.BackgroundColor = XLColor.LightGray;

			int row = 6;
			for (int i = 0; i < tutorials.Count; i++)
			{
				var t = tutorials[i];
				ws.Cell(row, 1).Value = i + 1;
				ws.Cell(row, 2).Value = t.Title ?? "";
				ws.Cell(row, 3).Value = Shorten(t.Content, 120);
				row++;
			}

			ws.Columns().AdjustToContents();
			ws.Column(3).Width = 60; // mô tả dài thì cho rộng

			using var ms = new MemoryStream();
			wb.SaveAs(ms);
			return ms.ToArray();
		}

		private static string Shorten(string? text, int max)
		{
			if (string.IsNullOrWhiteSpace(text)) return "";
			text = text.Replace("\r", " ").Replace("\n", " ").Trim();
			return text.Length <= max ? text : text.Substring(0, max) + "...";
		}
	}
}