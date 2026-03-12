using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HuongDanLamDep.Data;
using HuongDanLamDep.Models;
using HuongDanLamDep.Services;

namespace HuongDanLamDep.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class TutorialsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly ITutorialPdfService _pdf;

		public TutorialsController(ApplicationDbContext context, ITutorialPdfService pdf)
		{
			_context = context;
			_pdf = pdf;
		}

		// GET: Admin/Tutorials
		public async Task<IActionResult> Index(string? search, int page = 1)
		{
			const int pageSize = 5;

			var query = _context.Tutorials
				.Include(t => t.Category)
				.AsNoTracking()
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(t => t.Title.Contains(search));
			}

			var totalItems = await query.CountAsync();
			var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

			if (page < 1) page = 1;
			if (totalPages > 0 && page > totalPages) page = totalPages;

			var data = await query
				.OrderByDescending(t => t.TutorialId)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.Search = search;
			ViewBag.Page = page;
			ViewBag.TotalPages = totalPages;

			return View(data);
		}

		// GET: Admin/Tutorials/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tutorial = await _context.Tutorials
				.Include(t => t.Category)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.TutorialId == id);

			if (tutorial == null)
			{
				return NotFound();
			}

			return View(tutorial);
		}

		// ✅ GET: Admin/Tutorials/ExportPdf/5
		public async Task<IActionResult> ExportPdf(int id)
		{
			var tutorial = await _context.Tutorials
				.AsNoTracking()
				.Include(t => t.Category)
				.FirstOrDefaultAsync(t => t.TutorialId == id);

			if (tutorial == null)
			{
				return NotFound();
			}

			var pdfBytes = _pdf.GenerateTutorialPdf(tutorial);
			var fileName = $"{SafeFileName(tutorial.Title)}.pdf";

			return File(pdfBytes, "application/pdf", fileName);
		}

		// GET: Admin/Tutorials/Create
		public IActionResult Create()
		{
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
			return View();
		}

		// POST: Admin/Tutorials/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("TutorialId,Title,Content,CategoryId")] Tutorial tutorial)
		{
			if (!ModelState.IsValid)
			{
				ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", tutorial.CategoryId);
				return View(tutorial);
			}

			_context.Add(tutorial);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// GET: Admin/Tutorials/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tutorial = await _context.Tutorials.FindAsync(id);
			if (tutorial == null)
			{
				return NotFound();
			}

			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", tutorial.CategoryId);
			return View(tutorial);
		}

		// POST: Admin/Tutorials/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("TutorialId,Title,Content,CategoryId")] Tutorial tutorial)
		{
			if (id != tutorial.TutorialId)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", tutorial.CategoryId);
				return View(tutorial);
			}

			try
			{
				_context.Update(tutorial);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TutorialExists(tutorial.TutorialId))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToAction(nameof(Index));
		}

		// GET: Admin/Tutorials/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tutorial = await _context.Tutorials
				.Include(t => t.Category)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.TutorialId == id);

			if (tutorial == null)
			{
				return NotFound();
			}

			return View(tutorial);
		}

		// POST: Admin/Tutorials/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var tutorial = await _context.Tutorials.FindAsync(id);

			if (tutorial != null)
			{
				_context.Tutorials.Remove(tutorial);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool TutorialExists(int id)
		{
			return _context.Tutorials.Any(e => e.TutorialId == id);
		}

		private static string SafeFileName(string? name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return "tutorial";

			foreach (var c in Path.GetInvalidFileNameChars())
				name = name.Replace(c, '_');

			return name.Trim();
		}
	}
}
