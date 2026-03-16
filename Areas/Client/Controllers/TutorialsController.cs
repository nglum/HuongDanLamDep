using HuongDanLamDep.Data;
using HuongDanLamDep.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HuongDanLamDep.Areas.Client.Controllers
{
	[Area("Client")]
	public class TutorialsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TutorialsController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(int? categoryId, string? search)
		{
			var query = _context.Tutorials
				.AsNoTracking()
				.Include(t => t.Category)
				.AsQueryable();

			if (categoryId.HasValue)
			{
				query = query.Where(t => t.CategoryId == categoryId.Value);
			}

			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(t => t.Title.Contains(search));
			}

			var tutorials = await query
				.OrderByDescending(t => t.TutorialId)
				.ToListAsync();

			ViewBag.Categories = await _context.Categories
				.AsNoTracking()
				.OrderBy(c => c.Name)
				.ToListAsync();

			ViewBag.CurrentCategoryId = categoryId;
			ViewBag.Search = search;

			return View(tutorials);
		}

		public async Task<IActionResult> Details(int id)
		{
			var tutorial = await _context.Tutorials
				.AsNoTracking()
				.Include(t => t.Category)
				.FirstOrDefaultAsync(t => t.TutorialId == id);

			if (tutorial == null)
			{
				return NotFound();
			}

			return View(tutorial);
		}


		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddCommentAjax(int tutorialId, string content)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return Json(new { success = false, message = "Bạn chưa nhập nội dung bình luận." });
			}

			var tutorial = await _context.Tutorials
				.FirstOrDefaultAsync(t => t.TutorialId == tutorialId);

			if (tutorial == null)
			{
				return Json(new { success = false, message = "Không tìm thấy bài viết để bình luận." });
			}

			var comment = new Comment
			{
				TutorialId = tutorialId,
				Content = content.Trim(),
				CreatedAt = DateTime.Now,
				UserName = User.Identity?.Name
			};

			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();

			return Json(new
			{
				success = true,
				message = "Bình luận đã được gửi thành công.",
				userName = string.IsNullOrWhiteSpace(comment.UserName) ? "Người dùng" : comment.UserName,
				createdAt = comment.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
				content = comment.Content,
				tutorialId = comment.TutorialId
			});
		}

	}
}