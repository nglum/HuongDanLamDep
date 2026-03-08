using Microsoft.AspNetCore.Mvc;

namespace HuongDanLamDep.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			ViewData["Title"] = "Admin Dashboard";
			return View();
		}
	}
}