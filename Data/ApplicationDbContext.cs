using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor: Accepts DbContextOptions<ApplicationDbContext> for dependency injection
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet properties for each entity
        public DbSet<Student> Students { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Master> Masters { get; set; }

        // Configure relationships and seed data (optional)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships between entities
            modelBuilder.Entity<Student>()
                .HasOne(s => s.University)
                .WithMany()
                .HasForeignKey(s => s.UniversityID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Faculty)
                .WithMany()
                .HasForeignKey(s => s.FacultyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Faculty>()
                .HasOne(f => f.University)
                .WithMany(u => u.Faculties)
                .HasForeignKey(f => f.UniversityID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Faculty)
                .WithMany(f => f.Courses)
                .HasForeignKey(c => c.FacultyID)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data (optional)
            SeedData(modelBuilder);
        }

        // Seed initial data into the database (optional)
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Universities
            modelBuilder.Entity<University>().HasData(
                new University { UniversityID = 1, UniversityName = "University of Technology" },
                new University { UniversityID = 2, UniversityName = "University of Science" }
            );

            // Seed Faculties
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty { FacultyID = 1, FacultyName = "Faculty of Engineering", UniversityID = 1 },
                new Faculty { FacultyID = 2, FacultyName = "Faculty of Computer Science", UniversityID = 1 },
                new Faculty { FacultyID = 3, FacultyName = "Faculty of Physics", UniversityID = 2 }
            );

            // Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentID = 1, StudentName = "John Doe", UniversityID = 1, FacultyID = 1, NumberOfUnits = 12 },
                new Student { StudentID = 2, StudentName = "Jane Smith", UniversityID = 1, FacultyID = 2, NumberOfUnits = 15 },
                new Student { StudentID = 3, StudentName = "Alice Johnson", UniversityID = 2, FacultyID = 3, NumberOfUnits = 10 }
            );

            // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseID = 1, CourseName = "Introduction to Programming", FacultyID = 2 },
                new Course { CourseID = 2, CourseName = "Mechanical Engineering Basics", FacultyID = 1 },
                new Course { CourseID = 3, CourseName = "Quantum Physics", FacultyID = 3 }
            );

            // Seed Masters
            modelBuilder.Entity<Master>().HasData(
                new Master { Id = Guid.NewGuid(), FirstName = "Michael", LastName = "Brown" },
                new Master { Id = Guid.NewGuid(), FirstName = "Sarah", LastName = "Wilson" }
            );
        }
    }
}