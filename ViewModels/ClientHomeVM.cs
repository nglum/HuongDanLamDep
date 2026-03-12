using HuongDanLamDep.Models;

namespace HuongDanLamDep.ViewModels
{
	public class ClientHomeVM
	{
		public List<Category> Categories { get; set; } = new();
		public List<Tutorial> LatestTutorials { get; set; } = new();
	}
}