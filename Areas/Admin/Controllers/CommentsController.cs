using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HuongDanLamDep.Data;

namespace HuongDanLamDep.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class CommentsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CommentsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Admin/Comments
		public async Task<IActionResult> Index()
		{
			var comments = await _context.Comments
				.Include(c => c.Tutorial)
				.AsNoTracking()
				.OrderByDescending(c => c.CreatedAt)
				.ToListAsync();

			return View(comments);
		}

		// GET: Admin/Comments/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var comment = await _context.Comments
				.Include(c => c.Tutorial)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.CommentId == id);

			if (comment == null) return NotFound();

			return View(comment);
		}

		// POST: Admin/Comments/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var comment = await _context.Comments.FindAsync(id);

			if (comment != null)
			{
				_context.Comments.Remove(comment);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}