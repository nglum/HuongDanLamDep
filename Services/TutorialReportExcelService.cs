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

			// Title
			ws.Cell(1, 1).Value = "BÁO CÁO TUTORIAL THEO CATEGORY";
			ws.Range(1, 1, 1, 3).Merge().Style.Font.Bold = true;
			ws.Cell(1, 1).Style.Font.FontSize = 14;

			// Meta
			ws.Cell(2, 1).Value = "Category:";
			ws.Cell(2, 2).Value = categoryName;

			ws.Cell(3, 1).Value = "Tổng số:";
			ws.Cell(3, 2).Value = tutorials.Count;

			// Header (row 5)
			ws.Cell(5, 1).Value = "STT";
			ws.Cell(5, 2).Value = "Tiêu đề";
			ws.Cell(5, 3).Value = "Mô tả ngắn";

			var header = ws.Range(5, 1, 5, 3);
			header.Style.Font.Bold = true;
			header.Style.Fill.BackgroundColor = XLColor.LightGray;
			header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
			header.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

			// Data
			int row = 6;
			for (int i = 0; i < tutorials.Count; i++)
			{
				var t = tutorials[i];

				ws.Cell(row, 1).Value = i + 1;
				ws.Cell(row, 2).Value = t.Title ?? "";
				ws.Cell(row, 3).Value = Shorten(t.Content, 120);

				row++;
			}

			// Border for data region
			if (row > 6)
			{
				var dataRange = ws.Range(6, 1, row - 1, 3);
				dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
				dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
				dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
				ws.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			}

			// Auto fit
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