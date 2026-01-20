using Microsoft.EntityFrameworkCore;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly SistepedDbContext _context;

        public AttendanceRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Grade)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Grade)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Grade)
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByGradeIdAsync(int gradeId)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Grade)
                .Where(a => a.GradeId == gradeId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Grade)
                .Where(a => a.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByGradeAndDateAsync(int gradeId, DateTime date)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Grade)
                .Where(a => a.GradeId == gradeId && a.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByStudentAndGradeAsync(int studentId, int gradeId)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Grade)
                .Where(a => a.StudentId == studentId && a.GradeId == gradeId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<Attendance> CreateAsync(Attendance attendance)
        {
            attendance.CreatedAt = DateTime.Now;
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(attendance.Id) ?? attendance;
        }

        public async Task<IEnumerable<Attendance>> CreateBulkAsync(IEnumerable<Attendance> attendances)
        {
            var attendanceList = attendances.ToList();
            foreach (var attendance in attendanceList)
            {
                attendance.CreatedAt = DateTime.Now;
            }
            
            _context.Attendances.AddRange(attendanceList);
            await _context.SaveChangesAsync();
            
            return attendanceList;
        }

        public async Task<Attendance?> UpdateAsync(Attendance attendance)
        {
            var existingAttendance = await _context.Attendances.FindAsync(attendance.Id);
            if (existingAttendance == null) return null;

            existingAttendance.Present = attendance.Present;
            existingAttendance.Date = attendance.Date;

            await _context.SaveChangesAsync();
            return await GetByIdAsync(existingAttendance.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int studentId, int gradeId, DateTime date)
        {
            return await _context.Attendances
                .AnyAsync(a => a.StudentId == studentId && a.GradeId == gradeId && a.Date.Date == date.Date);
        }
    }
}
