using System.ComponentModel.DataAnnotations;

namespace HuongDanLamDep.Models
{
	public class Category
	{
		public int CategoryId { get; set; }

		[Required]
		public string Name { get; set; }
	}
}