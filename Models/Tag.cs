using System.ComponentModel.DataAnnotations;

namespace HuongDanLamDep.Models
{
	public class Tag
	{
		public int TagId { get; set; }

		[Required(ErrorMessage = "Bạn chưa nhập tên Tag")]
		public string Name { get; set; } = "";
	}
}