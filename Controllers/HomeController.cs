using Microsoft.AspNetCore.Mvc;

namespace HuongDanLamDep.Controllers
{
	public class HomeController : Controller
	{
		// Vào "/" sẽ chuyển sang trang Client
		public IActionResult Index()
			=> RedirectToAction("Index", "Home", new { area = "Client" });
	}
}
