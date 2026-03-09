using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace HuongDanLamDep.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			ViewData["Title"] = "Admin Dashboard";
			return View();
		}
	}
}