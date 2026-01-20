using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SistepedDbContext _context;

        public StudentRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Guardian)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student?> GetByEnrollmentAsync(string enrollment)
        {
            return await _context.Students
                .Include(s => s.Guardian)
                .FirstOrDefaultAsync(s => s.Enrollment == enrollment);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.Guardian)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetByGuardianIdAsync(int guardianId)
        {
            return await _context.Students
                .Include(s => s.Guardian)
                .Where(s => s.GuardianId == guardianId)
                .ToListAsync();
        }

        public async Task<Student> CreateAsync(Student student)
        {
            student.CreatedAt = DateTime.Now;
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(student.Id) ?? student;
        }

        public async Task<Student?> UpdateAsync(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.Id);
            if (existingStudent == null) return null;

            existingStudent.Enrollment = student.Enrollment;
            existingStudent.Name = student.Name;
            existingStudent.BirthDate = student.BirthDate;
            existingStudent.GuardianId = student.GuardianId;
            existingStudent.Status = student.Status;
            existingStudent.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(existingStudent.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }
    }
}
