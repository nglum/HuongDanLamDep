using Microsoft.AspNetCore.Mvc;

namespace HuongDanLamDep.Areas.Client.Controllers
{
	[Area("Client")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}