using System.ComponentModel.DataAnnotations;

namespace HuongDanLamDep.Models
{
	public class Tutorial
	{
		public int TutorialId { get; set; }

		[Required(ErrorMessage = "Bạn chưa nhập tiêu đề")]
		[StringLength(150)]
		public string Title { get; set; } = "";

		[Required(ErrorMessage = "Bạn chưa nhập nội dung")]
		public string Content { get; set; } = "";

		// FK -> Category
		[Display(Name = "Category")]
		[Required(ErrorMessage = "Bạn chưa chọn Category")]
		public int CategoryId { get; set; }

		public Category? Category { get; set; }
	}
}