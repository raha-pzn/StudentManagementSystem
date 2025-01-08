using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class UniversitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UniversitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Universities/Faculties/5
        public IActionResult Faculties(int id)
        {
            var university = _context.Universities
                .Include(u => u.Faculties) 
                .FirstOrDefault(u => u.UniversityID == id);

            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }
    }
}