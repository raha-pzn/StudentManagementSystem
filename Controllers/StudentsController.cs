using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public StudentsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: /Students
        public IActionResult Index()
        {
            var students = _context.Students
                .Include(s => s.University)
                .Include(s => s.Faculty)
                .ToList();
            return View(students);
        }

        // GET: /Students/Details/5
        public IActionResult Details(int id)
        {
            var student = _context.Students
                .Include(s => s.University)
                .Include(s => s.Faculty)
                .FirstOrDefault(s => s.StudentID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: /Students/Create
        public IActionResult Create()
        {
            // Populate ViewBag with universities and faculties for dropdowns
            ViewBag.Universities = _context.Universities
                .Select(u => new SelectListItem
                {
                    Value = u.UniversityID.ToString(),
                    Text = u.UniversityName
                })
                .ToList();

            ViewBag.Faculties = _context.Faculties
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();

            return View();
        }

        // POST: /Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                // Start a transaction
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Add the student to the database
                        _context.Students.Add(student);
                        await _context.SaveChangesAsync();

                        // Commit the transaction
                        await transaction.CommitAsync();

                        // Redirect to the list of students
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if an error occurs
                        await transaction.RollbackAsync();

                        // Log the error (optional)
                        // _logger.LogError(ex, "Error adding student");

                        // Return to the create view with an error message
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the student.");
                        return View(student);
                    }
                }
            }

            // If the model state is invalid, repopulate the dropdowns and return to the create view
            ViewBag.Universities = _context.Universities
                .Select(u => new SelectListItem
                {
                    Value = u.UniversityID.ToString(),
                    Text = u.UniversityName
                })
                .ToList();

            ViewBag.Faculties = _context.Faculties
                .Select(f => new SelectListItem
                {
                    Value = f.FacultyID.ToString(),
                    Text = f.FacultyName
                })
                .ToList();

            return View(student);
        }

        // GET: /Students/Names
        public async Task<IActionResult> Names()
        {
            var studentNames = new List<string>();

            // Get the connection string from appsettings.json
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Execute the stored procedure
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetStudentNames", connection))
                {
                    // Specify that this is a stored procedure
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Execute the stored procedure
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string name = reader["StudentName"].ToString();
                            studentNames.Add(name);
                        }
                    }
                }
            }

            // Pass the list of student names to the view
            return View(studentNames);
        }
    }
}