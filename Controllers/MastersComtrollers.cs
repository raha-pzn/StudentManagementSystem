using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class MastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Masters
        public async Task<IActionResult> Index()
        {
            var masters = await _context.Masters.ToListAsync();
            return View(masters);
        }

        // GET: /Masters/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var master = await _context.Masters
                .FirstOrDefaultAsync(m => m.Id == id);

            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // GET: /Masters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Masters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Master master)
        {
            if (ModelState.IsValid)
            {
                // Manually generate a GUID for the new master
                master.Id = Guid.NewGuid();

                _context.Masters.Add(master);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(master);
        }

        // GET: /Masters/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var master = await _context.Masters.FindAsync(id);

            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // POST: /Masters/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Master master)
        {
            if (id != master.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(master);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MasterExists(master.Id))
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

            return View(master);
        }

        // GET: /Masters/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var master = await _context.Masters
                .FirstOrDefaultAsync(m => m.Id == id);

            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // POST: /Masters/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var master = await _context.Masters.FindAsync(id);
            _context.Masters.Remove(master);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MasterExists(Guid id)
        {
            return _context.Masters.Any(e => e.Id == id);
        }
    }
}