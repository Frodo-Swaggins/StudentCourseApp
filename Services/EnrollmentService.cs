using Microsoft.EntityFrameworkCore;
using StudentCourseApp.Data;
using StudentCourseApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace StudentCourseApp.Services
{
    public class EnrollmentService
    {

        public readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            this._context = context;
        }

        //add new enrollment
        public async Task<bool> Enroll(int studentId, int courseId, string grade, decimal percebtage)
        {
            List<Student> enrolledStudents = await GetStudentsInCourse(courseId);
            if (! enrolledStudents.Any(s => s.Id == studentId) )
            {
                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    Grade = grade,
                    Percentage = percebtage,
                    IsActive = true
                };
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        //get all active enrollments
        public async Task<List<Enrollment>> GetActiveEnrollments()
        {
            return await _context.Enrollments
                .Where(e => e.IsActive)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();
        }

        //change grade
        public async Task ChangeGrade(int Id, string newGrade, decimal newPercentage)
        {
            var enrollment = await _context.Enrollments.FindAsync(Id);
            if (enrollment != null)
            {
                enrollment.Grade = newGrade;
                enrollment.Percentage = newPercentage;
                await _context.SaveChangesAsync();
            }
        }


        //deactivate enrollment
        public async Task Deactivate(Enrollment enrollment)
        {
            enrollment.IsActive = false;
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        //get students enrolled in a specific course
        public async Task<List<Student>> GetStudentsInCourse(int courseId)
        {
            return await _context.Students
                .Where(s => s.Enrollments.Any(e => e.CourseId == courseId && e.IsActive))
                .ToListAsync();
        }

    }
}
