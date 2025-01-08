using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "University is required.")]
        [Display(Name = "University")]
        public int UniversityID { get; set; }

        [Required(ErrorMessage = "Faculty is required.")]
        [Display(Name = "Faculty")]
        public int FacultyID { get; set; }

        [Required(ErrorMessage = "Number of Units is required.")]
        [Range(1, 24, ErrorMessage = "Number of units must be between 1 and 24.")]
        [Display(Name = "Number of Units")]
        public int NumberOfUnits { get; set; }

        // Navigation properties
        public University? University { get; set; }
        public Faculty? Faculty { get; set; }
    }
}