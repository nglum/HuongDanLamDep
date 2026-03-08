using Microsoft.AspNetCore.Mvc;

namespace WebHuongDanLamDep.Areas.Admin.Controllers
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