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
        public async Task<IActionResult> UpdateFacultyName(int id, [Bind("FacultyID,FacultyName,UniversityID")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Log the values for debugging
                    Console.WriteLine($"FacultyID: {faculty.FacultyID}, FacultyName: {faculty.FacultyName}, UniversityID: {faculty.UniversityID}");

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
                            command.Parameters.AddWithValue("@FacultyID", faculty.FacultyID);
                            command.Parameters.AddWithValue("@NewFacultyName", faculty.FacultyName);

                            // Execute the stored procedure
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    // Redirect to the Details page to see the updated faculty
                    return RedirectToAction(nameof(Details), new { id = faculty.FacultyID });
                }
                catch (SqlException ex)
                {
                    // Log the SQL exception
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the faculty name. Please try again later.");
                }
                catch (Exception ex)
                {
                    // Log the general exception
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }
            else
            {
                // Log ModelState errors for debugging
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"ModelState Error - Key: {key}, Error: {error.ErrorMessage}");
                    }
                }

                // Add a general error message for the user
                ModelState.AddModelError(string.Empty, "Please correct the errors and try again.");
            }

            // If the model state is invalid or an exception occurred, return to the form with validation errors
            return View(faculty);
        }
    }
}