namespace StudentManagementSystem.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        // Foreign key for faculty
        public int FacultyID { get; set; }
        public Faculty Faculty { get; set; }
    }
}