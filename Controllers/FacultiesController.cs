using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace StudentManagementSystem.Controllers
{
    public class FacultiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public FacultiesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: /Faculties/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var faculty = await _context.Faculties
                .Include(f => f.University) // Include the University
                .Include(f => f.Courses)   // Include the Courses
                .FirstOrDefaultAsync(f => f.FacultyID == id);

            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // GET: /Faculties/UpdateFacultyName/5
        public async Task<IActionResult> UpdateFacultyName(int id)
        {
            var faculty = await _context.Faculties
                .Include(f => f.University)
                .FirstOrDefaultAsync(f => f.FacultyID == id);

            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: /Faculties/UpdateFacultyName/5
        [HttpPost]
        public async Task<IActionResult> UpdateFacultyName(int id, string newFacultyName)
        {
            if (string.IsNullOrEmpty(newFacultyName))
            {
                ModelState.AddModelError("FacultyName", "Faculty name cannot be empty.");
                return View(await _context.Faculties.FindAsync(id));
            }

            // Get the connection string from appsettings.json
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("UpdateFacultyName", connection))
                {
                    // Specify that this is a stored procedure
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@FacultyID", id);
                    command.Parameters.AddWithValue("@NewFacultyName", newFacultyName);

                    // Execute the stored procedure
                    await command.ExecuteNonQueryAsync();
                }
            }

            // Redirect to the Details page to see the updated faculty
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}