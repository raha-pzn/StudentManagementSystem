using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Faculty
    {
        [Required(ErrorMessage = "Faculty ID is required.")]
        public int FacultyID { get; set; }

        [Required(ErrorMessage = "Faculty name is required.")]
        [StringLength(100, ErrorMessage = "Faculty name cannot exceed 100 characters.")]
        public string FacultyName { get; set; }

        // Foreign key for university
        public int UniversityID { get; set; }
        public University? University { get; set; }

        // Navigation property for courses
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}