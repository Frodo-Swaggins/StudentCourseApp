using Microsoft.EntityFrameworkCore;
using StudentCourseApp.Data;
using StudentCourseApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace StudentCourseApp.Services
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            this._context = context;
        }

        //get all students
        public async Task<List<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        //get all student with their enrollments
        public async Task<List<Student>> GetAllWithEnrollments()
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ToListAsync();
        }

        //get student by id
        public async Task<Student> GetById(int studentId)
        {
            return await _context.Students.FindAsync(studentId);
        }

        //get all students that are enrolled in a specific course
        public async Task<List<Student>> GetByCourseId(int courseId)
        {
            return await _context.Students
                .Where(s => s.Enrollments.Any(e => e.CourseId == courseId))
                .ToListAsync();
        }

        //get all students enrolled in any course
        public async Task<List<Student>> GetEnrolledStudents()
        {
            return await _context.Students
                .Where(s => s.Enrollments.Any())
                .ToListAsync();
        }


        //get student by name
        public async Task<Student> GetByName(string name)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Name == name);
        }

        //add new student
        public async Task<bool> Add(Student student)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (Regex.IsMatch(student.Email, pattern))
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        //update existing student
        public async Task Update(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        //delete student by id
        public async Task DeleteById(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

    }
}
