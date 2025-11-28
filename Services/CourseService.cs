using Microsoft.EntityFrameworkCore;
using StudentCourseApp.Data;
using StudentCourseApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace StudentCourseApp.Services
{
    public class CourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            this._context = context;
        }

        //get all courses
        public async Task<List<Course>> GetAll()
        {
            return await _context.Courses.ToListAsync();
        }

        // Get all courses with enrolled students
        public async Task<List<Course>> GetAllWithEnrollments()
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .ToListAsync();
        }


        //get course by id
        public async Task<Course> GetById(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        //add new course
        public async Task Add(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        //get all students enrolled in a specific course
        public async Task<List<Student>> GetEnrolledStudents(int courseId)
        {
            return await _context.Students
                .Where(s => s.Enrollments.Any(e => e.CourseId == courseId))
                .ToListAsync();
        }
        // Generate report data for a course
        public async Task<CourseReportViewModel> GenerateReport(int courseId)
        {
            var course = await GetById(courseId);
            if (course == null)
                return null;

            var studentGrades = course.Enrollments
                .Where(e => e.IsActive)
                .Select(e => new StudentGrade
                {
                    StudentName = e.Student.Name,
                    Grade = e.Grade,
                    Percentage = e.Percentage
                })
                .ToList();

            var numericGrades = studentGrades
                .Select(s => s.Percentage)
                .ToList();

            decimal average = numericGrades.Count > 0 ? numericGrades.Average() : 0;

            return new CourseReportViewModel
            {
                CourseTitle = course.Title,
                Description = course.Description,
                Credits = course.Credits,
                StudentGrades = studentGrades,
                ClassAverage = average
            };
        }

        //delete course by id
        public async Task Delete(int id)
        {
            var course = await GetById(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }
    }



    // ViewModel for report
    public class CourseReportViewModel
    {
        public string CourseTitle { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
        public List<StudentGrade> StudentGrades { get; set; } = new();
        public decimal ClassAverage { get; set; }
    }

    public class StudentGrade
    {
        public string StudentName { get; set; }
        public string Grade { get; set; }
        public decimal Percentage { get; set; }
    }

}
