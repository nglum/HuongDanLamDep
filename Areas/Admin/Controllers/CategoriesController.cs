using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HuongDanLamDep.Data;
using HuongDanLamDep.Models;
using HuongDanLamDep.Services;

namespace HuongDanLamDep.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoriesController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly ITutorialReportPdfService _pdf;
		private readonly ITutorialReportExcelService _excel;

		public CategoriesController(
			ApplicationDbContext context,
			ITutorialReportPdfService pdf,
			ITutorialReportExcelService excel)
		{
			_context = context;
			_pdf = pdf;
			_excel = excel;
		}

		// GET: Admin/Categories
		public async Task<IActionResult> Index(string? search, int page = 1)
		{
			const int pageSize = 5;

			var query = _context.Categories.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(search))
				query = query.Where(c => c.Name.Contains(search));

			var totalItems = await query.CountAsync();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			if (page < 1) page = 1;
			if (totalPages > 0 && page > totalPages) page = totalPages;

			var data = await query
				.OrderByDescending(c => c.CategoryId)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.Search = search;
			ViewBag.Page = page;
			ViewBag.TotalPages = totalPages;

			return View(data);
		}

		// GET: Admin/Categories/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var category = await _context.Categories
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.CategoryId == id);

			if (category == null) return NotFound();

			return View(category);
		}

		// ✅ GET: Admin/Categories/ExportTutorialsPdf/5
		public async Task<IActionResult> ExportTutorialsPdf(int id)
		{
			var category = await _context.Categories.AsNoTracking()
				.FirstOrDefaultAsync(c => c.CategoryId == id);

			if (category == null) return NotFound();

			var tutorials = await _context.Tutorials.AsNoTracking()
				.Where(t => t.CategoryId == id)
				.OrderByDescending(t => t.TutorialId)
				.ToListAsync();

			var bytes = _pdf.GenerateCategoryReport(category.Name, tutorials);
			var fileName = $"Tutorials_{SafeFileName(category.Name)}.pdf";

			return File(bytes, "application/pdf", fileName);
		}

		// ✅ GET: Admin/Categories/ExportTutorialsExcel/5
		public async Task<IActionResult> ExportTutorialsExcel(int id)
		{
			var category = await _context.Categories.AsNoTracking()
				.FirstOrDefaultAsync(c => c.CategoryId == id);

			if (category == null) return NotFound();

			var tutorials = await _context.Tutorials.AsNoTracking()
				.Where(t => t.CategoryId == id)
				.OrderByDescending(t => t.TutorialId)
				.ToListAsync();

			var bytes = _excel.GenerateCategoryReport(category.Name, tutorials);
			var fileName = $"Tutorials_{SafeFileName(category.Name)}.xlsx";

			return File(bytes,
				"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				fileName);
		}

		// GET: Admin/Categories/Create
		public IActionResult Create() => View();

		// POST: Admin/Categories/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CategoryId,Name")] Category category)
		{
			if (!ModelState.IsValid) return View(category);

			_context.Add(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// GET: Admin/Categories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var category = await _context.Categories.FindAsync(id);
			if (category == null) return NotFound();

			return View(category);
		}

		// POST: Admin/Categories/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name")] Category category)
		{
			if (id != category.CategoryId) return NotFound();

			if (!ModelState.IsValid) return View(category);

			try
			{
				_context.Update(category);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CategoryExists(category.CategoryId)) return NotFound();
				throw;
			}

			return RedirectToAction(nameof(Index));
		}

		// GET: Admin/Categories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var category = await _context.Categories
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.CategoryId == id);

			if (category == null) return NotFound();

			return View(category);
		}

		// POST: Admin/Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category != null)
				_context.Categories.Remove(category);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// ✅ CHỈ GIỮ 1 HÀM NÀY (xóa cái NotImplementedException)
		private static string SafeFileName(string? name)
		{
			if (string.IsNullOrWhiteSpace(name)) return "category";

			foreach (var c in Path.GetInvalidFileNameChars())
				name = name.Replace(c, '_');

			return name.Trim();
		}

		private bool CategoryExists(int id)
			=> _context.Categories.Any(e => e.CategoryId == id);
	}
}