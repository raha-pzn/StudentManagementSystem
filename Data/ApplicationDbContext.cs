using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<University> Universities { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Faculty>()
                .HasMany(f => f.Courses)
                .WithOne(c => c.Faculty)
                .HasForeignKey(c => c.FacultyID);
        }
    }
}