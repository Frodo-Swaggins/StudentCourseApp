using Microsoft.EntityFrameworkCore;

namespace StudentCourseApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<StudentCourseApp.Models.Student> Students { get; set; }
        public DbSet<StudentCourseApp.Models.Course> Courses { get; set; }
        public DbSet<StudentCourseApp.Models.Enrollment> Enrollments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the many-many relationships
            modelBuilder.Entity<StudentCourseApp.Models.Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<StudentCourseApp.Models.Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);
        }
    }
}
