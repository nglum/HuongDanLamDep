using HuongDanLamDep.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HuongDanLamDep.Areas.Client.Controllers
{
	[Area("Client")]
	public class TutorialsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TutorialsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Danh sách bài viết
		public async Task<IActionResult> Index(int? categoryId, string? search)
		{
			var query = _context.Tutorials
				.AsNoTracking()
				.Include(t => t.Category)
				.AsQueryable();

			if (categoryId.HasValue)
			{
				query = query.Where(t => t.CategoryId == categoryId.Value);
			}

			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(t => t.Title.Contains(search));
			}

			var tutorials = await query
				.OrderByDescending(t => t.TutorialId)
				.ToListAsync();

			ViewBag.Categories = await _context.Categories
				.AsNoTracking()
				.OrderBy(c => c.Name)
				.ToListAsync();

			ViewBag.CurrentCategoryId = categoryId;
			ViewBag.Search = search;

			return View(tutorials);
		}

		// Chi tiết bài viết
		public async Task<IActionResult> Details(int id)
		{
			var tutorial = await _context.Tutorials
				.AsNoTracking()
				.Include(t => t.Category)
				.FirstOrDefaultAsync(t => t.TutorialId == id);

			if (tutorial == null)
			{
				return NotFound();
			}

			return View(tutorial);
		}
	}
}
