using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class University
    {
        public int UniversityID { get; set; }

        [Required(ErrorMessage = "University name is required.")]
        [StringLength(100, ErrorMessage = "University name cannot exceed 100 characters.")]
        public string UniversityName { get; set; }

        // Navigation property for faculties
        public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
    }
}