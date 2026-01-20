using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class StudentGradeRepository : IStudentGradeRepository
    {
        private readonly SistepedDbContext _context;

        public StudentGradeRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<StudentGrade?> GetByIdAsync(int id)
        {
            return await _context.StudentGrades
                .Include(sg => sg.Student)
                .Include(sg => sg.Grade)
                .FirstOrDefaultAsync(sg => sg.Id == id);
        }

        public async Task<IEnumerable<StudentGrade>> GetAllAsync()
        {
            return await _context.StudentGrades
                .Include(sg => sg.Student)
                .Include(sg => sg.Grade)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentGrade>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentGrades
                .Include(sg => sg.Student)
                .Include(sg => sg.Grade)
                .Where(sg => sg.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentGrade>> GetByGradeIdAsync(int gradeId)
        {
            return await _context.StudentGrades
                .Include(sg => sg.Student)
                .Include(sg => sg.Grade)
                .Where(sg => sg.GradeId == gradeId)
                .ToListAsync();
        }

        public async Task<StudentGrade> CreateAsync(StudentGrade studentGrade)
        {
            studentGrade.CreatedAt = DateTime.Now;
            _context.StudentGrades.Add(studentGrade);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(studentGrade.Id) ?? studentGrade;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var studentGrade = await _context.StudentGrades.FindAsync(id);
            if (studentGrade == null) return false;

            _context.StudentGrades.Remove(studentGrade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int studentId, int gradeId)
        {
            return await _context.StudentGrades
                .AnyAsync(sg => sg.StudentId == studentId && sg.GradeId == gradeId);
        }
    }
}
