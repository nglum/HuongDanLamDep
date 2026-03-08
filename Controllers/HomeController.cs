using Microsoft.AspNetCore.Mvc;

namespace WebHuongDanLamDep.Controllers
{
	public class HomeController : Controller
	{
		// Vào "/" sẽ chuyển sang trang Client
		public IActionResult Index()
			=> RedirectToAction("Index", "Home", new { area = "Client" });
	}
}
