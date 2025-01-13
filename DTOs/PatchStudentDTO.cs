namespace StudentManagementSystem.DTOs
{
    public class PatchStudentDTO
    {
        public string StudentName { get; set; } // Nullable to allow partial updates
        public int? UniversityID { get; set; }   // Nullable to allow partial updates
        public int? FacultyID { get; set; }      // Nullable to allow partial updates
        public int? NumberOfUnits { get; set; }  // Nullable to allow partial updates
    }
}