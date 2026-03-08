using System.ComponentModel.DataAnnotations;

namespace HuongDanLamDep.Models
{
	public class Category
	{
		public int CategoryId { get; set; }

		[Required(ErrorMessage = "Bạn chưa nhập tên Category")]
		public string Name { get; set; } = "";
	}
}
