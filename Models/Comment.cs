using System;
using System.ComponentModel.DataAnnotations;

namespace HuongDanLamDep.Models
{
	public class Comment
	{
		public int CommentId { get; set; }

		[Required]
		public string Content { get; set; } = "";

		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public int TutorialId { get; set; }
		public Tutorial? Tutorial { get; set; }

		public string? UserName { get; set; }
	}
}
