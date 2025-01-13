using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Master
    {
        [Key]
        public Guid Id { get; set; } // GUID as the primary key

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }
    }
}