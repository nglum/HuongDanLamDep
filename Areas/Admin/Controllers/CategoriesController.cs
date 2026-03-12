using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HuongDanLamDep.Data;
using HuongDanLamDep.Models;

namespace HuongDanLamDep.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CategoriesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CategoriesController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Admin/Categories
		public async Task<IActionResult> Index(string? search, int page = 1)
		{
			const int pageSize = 5;

			var query = _context.Categories.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(c => c.Name.Contains(search));
			}

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
			if (id == null)
				return NotFound();

			var category = await _context.Categories
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.CategoryId == id);

			if (category == null)
				return NotFound();

			return View(category);
		}

		// GET: Admin/Categories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Admin/Categories/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CategoryId,Name")] Category category)
		{
			if (!ModelState.IsValid)
				return View(category);

			_context.Add(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// GET: Admin/Categories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var category = await _context.Categories.FindAsync(id);
			if (category == null)
				return NotFound();

			return View(category);
		}

		// POST: Admin/Categories/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name")] Category category)
		{
			if (id != category.CategoryId)
				return NotFound();

			if (!ModelState.IsValid)
				return View(category);

			try
			{
				_context.Update(category);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CategoryExists(category.CategoryId))
					return NotFound();

				throw;
			}

			return RedirectToAction(nameof(Index));
		}

		// GET: Admin/Categories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			var category = await _context.Categories
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.CategoryId == id);

			if (category == null)
				return NotFound();

			return View(category);
		}

		// POST: Admin/Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var category = await _context.Categories.FindAsync(id);

			if (category != null)
			{
				_context.Categories.Remove(category);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CategoryExists(int id)
		{
			return _context.Categories.Any(e => e.CategoryId == id);
		}
	}
}