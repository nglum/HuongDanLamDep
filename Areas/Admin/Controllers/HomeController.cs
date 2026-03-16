using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HuongDanLamDep.Data;

namespace HuongDanLamDep.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			ViewBag.CategoryCount = await _context.Categories.CountAsync();
			ViewBag.TutorialCount = await _context.Tutorials.CountAsync();
			ViewBag.CommentCount = await _context.Comments.CountAsync();

			return View();
		}
	}
}
