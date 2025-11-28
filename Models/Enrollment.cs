using System;
using System.ComponentModel.DataAnnotations;

namespace StudentCourseApp.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;

        // Foreign keys
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        [MaxLength(5)]
        public string Grade { get; set; }   

        [Range(0, 100)]
        public decimal Percentage { get; set; }   

        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
