namespace StudentManagementSystem.DTOs
{
    public class StudentDTO
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int UniversityID { get; set; }
        public int FacultyID { get; set; }
        public int NumberOfUnits { get; set; }
        public UniversityDTO University { get; set; }
        public FacultyDTO Faculty { get; set; }
    }
}