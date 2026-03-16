using HuongDanLamDep.Data;
using HuongDanLamDep.Models;
using HuongDanLamDep.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HuongDanLamDep.Areas.Client.Controllers
{
	[Area("Client")]
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var vm = new ClientHomeVM
			{
				Categories = await _context.Categories
					.AsNoTracking()
					.OrderBy(c => c.Name)
					.ToListAsync(),

				LatestTutorials = await _context.Tutorials
					.AsNoTracking()
					.Include(t => t.Category)
					.OrderByDescending(t => t.TutorialId)
					.Take(6)
					.ToListAsync()
			};

			ViewBag.Comments = await _context.Comments
				.AsNoTracking()
				.OrderByDescending(c => c.CreatedAt)
				.ToListAsync();

			return View(vm);
		}
	}
}