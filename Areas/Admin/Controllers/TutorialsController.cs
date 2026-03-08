using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HuongDanLamDep.Data;
using HuongDanLamDep.Models;

namespace HuongDanLamDep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TutorialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TutorialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Tutorials
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tutorials.Include(t => t.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Tutorials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorial = await _context.Tutorials
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TutorialId == id);
            if (tutorial == null)
            {
                return NotFound();
            }

            return View(tutorial);
        }

        // GET: Admin/Tutorials/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Admin/Tutorials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TutorialId,Title,Content,CategoryId")] Tutorial tutorial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutorial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", tutorial.CategoryId);
            return View(tutorial);
        }

        // GET: Admin/Tutorials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorial = await _context.Tutorials.FindAsync(id);
            if (tutorial == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", tutorial.CategoryId);
            return View(tutorial);
        }

        // POST: Admin/Tutorials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TutorialId,Title,Content,CategoryId")] Tutorial tutorial)
        {
            if (id != tutorial.TutorialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorialExists(tutorial.TutorialId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", tutorial.CategoryId);
            return View(tutorial);
        }

        // GET: Admin/Tutorials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorial = await _context.Tutorials
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TutorialId == id);
            if (tutorial == null)
            {
                return NotFound();
            }

            return View(tutorial);
        }

        // POST: Admin/Tutorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutorial = await _context.Tutorials.FindAsync(id);
            if (tutorial != null)
            {
                _context.Tutorials.Remove(tutorial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorialExists(int id)
        {
            return _context.Tutorials.Any(e => e.TutorialId == id);
        }
    }
}
