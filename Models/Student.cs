using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentCourseApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
